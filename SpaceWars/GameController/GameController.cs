using NetworkController;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Model;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace GameController
{
    public delegate void FrameTick();
    public delegate void KeyEvent();
    public delegate void closeEvent();

    public class GameController
    {
        //initialize player neme.
        private string name;
        //initialize different parts we send through the socket.
        private List<string> parts = new List<string>();
        //initialize world class.
        private World world;
        //initialize frametick event.
        private event FrameTick Frametick;
        //initializing the ID.
        private int ID;
        //initializing the size.
        private int size;
        //initializing the socket.
        private Socket socket;
        //initializing the event keyevemt.
        private event KeyEvent keyevent;
        //initializing the event closeevent.
        private event closeEvent closeevent;
        
        //Delegate to handle event coming from Networking class.
        public delegate void GameExceptionEventHandler();
        private static event GameExceptionEventHandler gameException1;

        /// <summary>
        /// Registering the eventhandler that comes from the networking class.
        /// </summary>
        /// <param name="e1"></param>
        public void registerNetwork1(GameExceptionEventHandler e1)
        {
            gameException1 += e1;
        }

        /// <summary>
        /// handling the networing exception that comes from the networking class.
        /// </summary>
        public static void handleNetworkException()
        {
            gameException1();
        }


        public GameController()
        {
            this.world = new World();
            Networking.registerNetwork(handleNetworkException);
        }

        /// <summary>
        /// This method return the world.
        /// </summary>
        /// <returns></returns>
        public World GetWorld()
        {
            return this.world;
        }
      
        /// <summary>
        /// This method registers the frametick and increments it.
        /// </summary>
        /// <param name="frametick"></param>
        public void RegisterFrame(FrameTick frametick)
        {
            Frametick += frametick;
        }

        /// <summary>
        /// This method registers an event.
        /// </summary>
        /// <param name="h"></param>
        public void RegisterEvent(KeyEvent h)
        {
            keyevent += h;
        }

        /// <summary>
        /// method to close a register.
        /// </summary>
        /// <param name="ce"></param>
        public void RegisterClose(closeEvent ce)
        {
            closeevent += ce;
        }
        
        /// <summary>
        /// This method sets name.
        /// </summary>
        /// <param name="name"></param>
        public void setName(string name)
        {
            this.name = name;
        }

      

        /// <summary>
        /// This method receives the startup and gets the data.
        /// </summary>
        /// <param name="ss"></param>
        public void ReceiveStartUp(SocketState ss)
        {
            //uses stringbuilder to convert ss to string and store in totaldata.
            string totalData = ss.sb.ToString();
            //we split total data by nextline and store that in array parts.
            string[] parts = totalData.Split('\n');
            //parses the 1st index and stores in id.
            this.ID = Int32.Parse(parts[0]);
            //parses the 2nd index and stores in size.
            this.size = Int32.Parse(parts[1]);
            world.worldSize = size;
            //remove 1st and 2nd index from socketstate.
            ss.sb.Remove(0, parts[0].Length) ;
            ss.sb.Remove(0, parts[1].Length);
            //using delegate callMe to recieve world.
            ss.callMe = ReceiveWorld;
            //closing an event.
            closeevent();
            //getting data from networking class.
            Networking.GetData(ss);
        }

       

        /// <summary>
        /// This is the method recieve the world, serialize it and split it by next line, Deserializes the totalDat
        /// and sets projectiles,ships and stars.
        /// </summary>
        /// <param name="ss"></param>
        public void ReceiveWorld(SocketState ss)
        {
            string totalData = ss.sb.ToString();

            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.MissingMemberHandling = MissingMemberHandling.Error;
            //this method splits totalData by nextline.
            String[] parts = Regex.Split(totalData, @"(?<=[\n])");
            //looping through parts.
            foreach(string p in parts)
            {
                if (p.Length == 0)
                    continue;
                if (p[p.Length - 1] != '\n')
                    break;
                if (p[0] == '{' || p[p.Length - 2] == '}')
                {
                    //parsing the jason object and setting it to obj.
                    JObject obj = JObject.Parse(p);

                    //different tokens in Jason object.
                    JToken shipToken = obj["ship"];
                    JToken projectileToken = obj["proj"];
                    JToken starToken = obj["star"];

                    Ship theship = null;
                    Projectile theProjectile = null;
                    Star thestar = null;

                    if (shipToken != null)
                    {
                        //Deserialiing the ship object.
                        theship = JsonConvert.DeserializeObject<Ship>(p);

                    }
                    if (projectileToken != null)
                    {
                        //Deserializing the Projectile object.
                        theProjectile = JsonConvert.DeserializeObject<Projectile>(p);

                    }
                    if (starToken != null)
                    {
                        //Deserializing the star object.
                       thestar = JsonConvert.DeserializeObject<Star>(p);

                    }

                    lock (world)
                    {
                        if (theship != null)
                        {
                            //storing all ships in ship collecting in world class.
                            world.shipCollection[theship.getShipId()] = theship;
                            
                        }
                        if (thestar != null)
                        {
                            //storing all star in star collecting in world class.
                            world.starCollection[thestar.getStarId()] = thestar;
                        }

                        if (theProjectile != null)
                        {
                            //removing only if the projectile is alive.
                            if (theProjectile.getProjectileAlive() == false)
                            {
                                //removing all projectiles in projectile collection from world class.
                                world.projectileCollection.Remove(theProjectile.getProjectileId());
                            }
                            else
                            { //storing all projectile in  projectilecollecting in world class.
                                world.projectileCollection[theProjectile.getProjectileId()] = theProjectile;
                            }
                        }
                    }

               
                }
                //Removing the socketstate.
                ss.sb.Remove(0, p.Length);
                Frametick();
                
            }
            keyevent();
            //calling the delegate call me to recieveworld.
            ss.callMe = ReceiveWorld;
            Networking.GetData(ss);

        }

        /// <summary>
        /// this method is invoked when the button is clicked.
        /// </summary>
        /// <param name="textBox1"></param>
        public void click(string textBox1)
        {
            try
            {
                this.socket = Networking.ConnectToServer(FirstContact, textBox1);
            }
            catch(Exception e)
            {
                
            }

        }

        /// <summary>
        /// Method that sends all the data to the socket.
        /// </summary>
        /// <param name="data"></param>
        public void Send(string data)
        {
            NetworkController.Networking.Send(this.socket, data);
        }
        
        /// <summary>
        /// this method makes the frist contact and puts a nextline before getting other datas.
        /// </summary>
        /// <param name="state"></param>
        public void FirstContact(SocketState state)
        {
            state.callMe = ReceiveStartUp;
            Networking.Send(state.theSocket, name  + "\n");

            Networking.GetData(state);
        }
    }
}
