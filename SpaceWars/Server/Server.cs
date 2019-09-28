using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkController;
using Model;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Xml;
using SpaceWars;
using GameController;
using System.Diagnostics;

namespace Server
{
    class Server
    {
        //Holds the UniverseSize
        private static int UniverseSize;
        //Holds MSPerFrame
        private static int MSPerFrame;
        //Holds FramesPerShot
        private static int FramesPerShot;
        //Holds RespawnRate.
        private static int RespawnRate;
        //Holds gameMode.
        private static int gameMode;
        //Holds x axis.
        private static int x;
        //Holds y axis.
        private static int y;
        //Holds the mass of the star.
        private static double mass;
        //Holds the Star.
        public static Star star;
        //Holds the world.
        public static World world;
        //Holds the watch.
        private static Stopwatch watch;
        //Holds the worldSize.
        private static Vector2D worldSize;
        //Holds and initializes id.
        private static int id = -1;
        //Holds the list of ss.
        public static List<SocketState> list;

        /// <summary>
        /// This is the main function that initializes everything, starts the task, 
        /// starts and ends the stopwatch and also handlesnew client.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            id = 0;
            //initializing the stopwatch.
            watch = new Stopwatch();
            //calling the XmlRead.
            XmlRead();
            //setting the world to the new world.
            world = new World();
            //setting the star collection to star.
            world.starCollection[star.getStarId()] = star;
            //inintializing the world size to the Vector x and y.
            worldSize = new Vector2D(x, y);
            list = new List<SocketState>();
            //Server awaiting client loop to handle a new client.
            Networking.ServerAwaitingClientLoop(HandleNewClient);
          
            //creating and initializing the Task.
            Task task = new Task(() =>
              {
                  //starting the watch.
                  watch.Start();
                  while (true)
                  {
                      //calling the Update method.
                      Update();
                      while (watch.ElapsedMilliseconds < MSPerFrame)
                      {

                     }
                      //restarting the watch.
                     watch.Restart();
                         
                  }

              });
            task.Start();
            Console.Read();
        }


        /// <summary>
        /// This method reads all the value from setting.xml file and stores it as a 
        /// global variables.
        /// </summary>
        public static void XmlRead()
        {
            try
            {
                using (XmlReader reader = XmlReader.Create("../../../Resources/settings.xml"))
                {
                    while (reader.Read())
                    {
                        //if UniverseSize is read store it in UniverseSize.
                        if (reader.IsStartElement())
                        {
                            if (reader.Name == "UniverseSize")
                            {
                                reader.Read();
                                UniverseSize = int.Parse(reader.Value);
                            }
                            //if MSPerFrame is read parses it into int and stores it.
                            if (reader.Name == "MSPerFrame")
                            {
                                reader.Read();
                                MSPerFrame = int.Parse(reader.Value);
                            }
                            //if FramesPerShot is read parses it into int and stores it.
                            if (reader.Name == "FramesPerShot")
                            {
                                reader.Read();
                                FramesPerShot = int.Parse(reader.Value);
                            }
                            //if RespawnRate is read parses it into int and stores it.
                            if (reader.Name == "RespawnRate")
                            {
                                reader.Read();
                                RespawnRate = int.Parse(reader.Value);
                            }
                            //if gamemode is read parses it into int and stores it.
                            if (reader.Name == "GameMode")
                            {
                                reader.Read();
                                gameMode = int.Parse(reader.Value);
                            }
                            //if star is read jumps into readStar method.
                            if (reader.Name == "Star")
                            {
                                readStar(reader);
                            }
                           
                        }
                    }
                }
            }
            //wrong settings is read throws an exception.
            catch (Exception e)
            {
                throw new Exception();
            }
        }


        /// <summary>
        /// This method goes inside star settings reads it and stores it.
        /// </summary>
        /// <param name="reader"></param>
        public static void readStar(XmlReader reader)
        {
            //boolean flags for x, y and mass.
            bool hasX = false;
            bool hasY = false;
            bool hasMass = false;

            try { 
                    while (reader.Read())
                    {
                         //if x is read parses it and stores it in x.
                        if (reader.IsStartElement())
                        {
                                if (reader.Name == "x")
                                {
                                    reader.Read();
                                    x = int.Parse(reader.Value);
                                    hasX = true;
                                }
                                //if y is read parses it and stores it.
                                if (reader.Name == "y")
                                {
                                     reader.Read();
                                      y = int.Parse(reader.Value);
                                      hasY = true;
                                }
                                // if mass is read parses it and stores it.
                                if (reader.Name == "mass")
                                {
                                     reader.Read();
                                     mass = Double.Parse(reader.Value);
                                     hasMass = true;
                                }
                                
                         }
                    }
                    //if star has x, y and mass we create a new location based on what the 
                    //values are.
                    if(hasX && hasY && hasMass)
                    {
                    Vector2D loc = new Vector2D(x, y);
                    star = new Star(0,loc,mass);
                    }
            }
            //if the file could not be read throw an exception.
            catch (Exception e)
            {
                throw new Exception();
            }
        }

        //delegate for recieveName.
        public delegate void ReceiveName();


        /// <summary>
        /// This method handles a new client.
        /// </summary>
        /// <param name="ss"></param>
        public static void HandleNewClient(SocketState ss)
        {
            //callme delegate that is set to recievename.
            ss.callMe = RecieveName;
            //get the data from the ss.
            Networking.GetData(ss);
        }


        /// <summary>
        /// This methods recieves the name and does the first handshake with the
        /// client.
        /// </summary>
        /// <param name="ss"></param>
        public static void RecieveName(SocketState ss)
        {
            
            string name = "";
         
            //totalData uses string builder to store all the string from the ss.
            string totalData = ss.sb.ToString();

            //this method splits totalData by nextline.
            String[] parts = totalData.Split('\n');
            //the first string sent is the players name.
            name = parts[0];
            Ship ship;
          
            //lock the world
            lock (world)
            {
                //calls generate new ship from world and sets it to ship.
                ship = world.generatenewShip(name);
                //sets ss.ID to ships id.
                ss.setSocketStateID(ship.getShipId());
            }

            ss.callMe = callBack;

            //lock the list and add the socketstate.
            lock (list)
            {
                list.Add(ss);
            }
            int universe = UniverseSize;
            //removes the name from the socketstate.
            ss.sb.Remove(0, parts[0].Length + 1);
            //sends data to the specific socket.
            Networking.Send(ss.theSocket, ss.ID.ToString() +"" + "\n" + universe + "\n");
            //calling get data to get more data.
            Networking.GetData(ss);
        }



        /// <summary>
        /// This method splits the ss string by next line and runs the command needed 
        /// for the game.
        /// </summary>
        /// <param name="ss"></param>
        public static void callBack(SocketState ss)
        {
            //total data uses stringbuilder to store the data comming from socket.
            string totalData = ss.sb.ToString();
             //splits total data into char array.
            char[] parts = totalData.ToCharArray();

            //if the ships hp is not zero.
            if (world.shipCollection[ss.ID].getShipHp() != 0)
            {
                //for each charater in char array parts.
                foreach (char temp in parts)
                {
                    //if the char is F
                    if (temp == 'F' )
                    {
                        //if just fired is not true.
                        if (!world.shipCollection[ss.ID].getJustFired())
                        {
                            world.shipCollection[ss.ID].justFired(true);
                            //create a new projctile and sets it attributes.
                            Projectile proj = new Projectile();
                            proj.setProjectileAlive(true);
                            proj.setProjectileVelocity(new Vector2D(0, 0));
                            proj.setProjectileDir(new Vector2D(world.shipCollection[ss.ID].getShipDir()));
                            proj.setProjectileID(generateID());
                            proj.setProjectileLoc(world.shipCollection[ss.ID].getShipLoc());
                            proj.setProjectileOwner(ss.ID);

                            lock (world)
                            {
                                //add the projectiles that was created to the collection.
                                world.projectileCollection.Add(proj.getProjectileId(), proj);
                            }
                        }
                    }
                    //if char is not open or closed paran run operateship method.
                    if (temp != '(' && temp != ')')
                    {
                        world.shipCollection[ss.ID].doOperateShip(temp);
                    }
                }
            }
            //clear the socketstate after.
            ss.sb.Clear();
            //get the data.
            Networking.GetData(ss);
        }


        /// <summary>
        /// this method generate a new id for the projectiles.
        /// </summary>
        /// <returns></returns>
        private static int generateID()
        {
            id++;
            return id;
        }
       
        /// <summary>
        /// This method updates the state of the world.
        /// </summary>
        private static void Update()
        {
           
            StringBuilder sb = new StringBuilder();
            //lock the world
            lock (world)
            {
                //for all star in star collection we appenfd to sb.
                foreach (Star s in world.starCollection.Values)
                {
                    sb.Append(s.ToString() + "\n");
                    
                }
                //method to update ships.
                updateships(sb);
                //methods that handles collisons.
                Collisons();
                //method that updates projctile.
                updateProjectile(sb);
            }

            lock (world)
            {
                //copy of the list to remove it from the ss.
                List<SocketState> copy = new List<SocketState>(list);
                
                foreach(SocketState ss in list)
                {
                    //if the client window is closed.
                    if ((Networking.Send(ss.theSocket, sb.ToString())) == false)
                    {
                        lock (world)
                        {
                            int id = ss.ID;
                            //set the ships hp tp zero.
                            world.shipCollection[ss.ID].setShipHp(0);
                            //and set ship disconnected to true.
                            world.shipCollection[ss.ID].setDisconnected(true);
                            //remove the ship from the ss.
                            copy.Remove(ss);
                           
                        }
                    }
                }
                //after removing ship from the client we set list back to copy.
                list = copy;
            }
        }


        /// <summary>
        /// This method updates the ship.
        /// </summary>
        /// <param name="sb"></param>
        private static void updateships(StringBuilder sb)
        {
            foreach (Ship s in world.shipCollection.Values)
            {
                //if ships hp is more than zero.
                if (s.getShipHp() > 0)
                {
                    //method to move ship.
                    moveShip(s);
                    //checks the position of the ship.
                    shipsPosition(s);
                    //if proj is equal to frames per shot.
                    if (s.getCooldownProj() == FramesPerShot)
                    {
                        //set just fired to false.
                        s.justFired(false);
                        s.resetCooldownProj();
                    }
                    else
                    {
                        //increment to proj count.
                        s.incrementCooldownProj();
                    }
                }
                else
                {
                    //if client universe is not dsconnected.
                    if (world.shipCollection[s.getShipId()].getDisconnected() == false)
                    {
                        //if respawnrate is equal to respawn counter.
                        if (s.getRespawnCounter() == RespawnRate)
                        {
                            //the ship is respawned and new location is choosen.
                            s.respawn(world.generateLocation(s));

                        }
                        else
                        {
                            //increment respawn counter.
                            s.incrementRespawnCounter();
                        }
                    }
                }
                //appending ships to the string builder.
                sb.Append(s.ToString() + '\n');
                //set ships thrust to false.
                s.setShipThrust(false);
                //sets ships operate right to false;
                s.setOperateRight(false);
                //sets ships operate left to false.
                s.setOperateLeft(false);
            }
        }

        /// <summary>
        /// this method checks if the ship is going out of the universe.
        /// </summary>
        /// <param name="s"></param>
        private static void shipsPosition(Ship s)
        {
            //if ships x is greater than universize by 2.
            if (s.getShipLoc().GetX() > (UniverseSize / 2))
            {
                //set ship to the opposite location.
                s.setShipLoc(new Vector2D((-UniverseSize / 2), (s.getShipLoc().GetY())));
            }

            //if ships y os greter than universesize by two
            if (s.getShipLoc().GetY() > (UniverseSize / 2))
            {
                //set ship to the opposite location.
                s.setShipLoc(new Vector2D((s.getShipLoc().GetX()), (-(UniverseSize / 2))));
            }
            //if ships x is less than universize by 2.
            if (s.getShipLoc().GetX() < -(UniverseSize / 2))
            {
                //set ship to the opposite location.
                s.setShipLoc(new Vector2D(((UniverseSize / 2)), s.getShipLoc().GetY()));
            }
            //if ships y os less than universesize by two
            if (s.getShipLoc().GetY() < -(UniverseSize / 2))
            {
                //set ship to the opposite location.
                s.setShipLoc(new Vector2D((s.getShipLoc().GetX()), (UniverseSize / 2)));
            }
        }

        /// <summary>
        /// This method updates the projectile location.
        /// </summary>
        /// <param name="sb"></param>
        private static void updateProjectile(StringBuilder sb)
        {
            //for every projectile p in projectile collection.
            foreach(Projectile p in world.projectileCollection.Values.ToList())
            {   
                //if gameMOde is set to 1.
                if (gameMode == 1)
                {
                    //method that evaluates gravity for projectile.
                    gravityForProjectile(p);
                }
                else
                {
                    //else the projectile loc is set in a normal way.
                    Vector2D velLoc = new Vector2D(p.getProjectileDir());
                    //normalize the projectile direction.
                    velLoc.Normalize();
                    //multiply veloc by 15 and set it back to veloc.
                    velLoc = velLoc * 15;
                    p.setProjectileLoc(velLoc + p.getProjectileLoc());
                }

                //if the projection is going out of the universe set projetileAlive to false and remove it from the dictionary.
                if (p.getProjectileLoc().GetX() > (UniverseSize / 2) || p.getProjectileLoc().GetX() < -(UniverseSize / 2) || p.getProjectileLoc().GetY() > (UniverseSize / 2) || p.getProjectileLoc().GetY() < -(UniverseSize / 2) || p.getProjectileAlive() is false)
                {
                    p.setProjectileAlive(false);
                    sb.Append(p.ToString() + '\n');
                    world.getProjectileDictionary().Remove(p.getProjectileId());
                }
                else
                {
                    //append projectile to the string builder.
                    sb.Append(p.ToString() + '\n');
                    if (p.getProjectileAlive() ==  false)
                    {
                        //remove projectile from the world.
                        world.getProjectileDictionary().Remove(p.getProjectileId());
                    }
                }
            }
        }

        /// <summary>
        /// This method is for the extra game mode that applies gravity to the projectile
        /// </summary>
        /// <param name="p"></param>
        private static void gravityForProjectile(Projectile p)
        {
            Vector2D velLoc = new Vector2D(p.getProjectileDir());
            velLoc.Normalize();
            velLoc = velLoc * 10;
            //g is gravity.
            Vector2D g = new Vector2D(0, 0);
            //gravity is star location minus projectile location.
            g = star.getStarLoc() - p.getProjectileLoc();
            //normalize gravity.
            g.Normalize();
            //sets gravities strength.
            g = g * 3.0;
            Vector2D a = new Vector2D(0, 0);
            //adding gravity with the thrust of the projectile.
            Vector2D t = new Vector2D(p.getProjectileDir());
            t = t * 0.8;
            a = g + t;


            p.setProjectileVelocity(p.getProjectileVelocity() + a);
            //setting projectile location with respect to the gravity.
            p.setProjectileLoc(velLoc + p.getProjectileLoc() + p.getProjectileVelocity());

        }


        /// <summary>
        /// This method calculates when the collison should occur.
        /// </summary>
        private static void Collisons()
        {
            foreach (Ship s in world.shipCollection.Values)
            {
                //method that calculates projectile and ship collision.
                ProjectileShipCollision(s);
                //method that calculates ship and star collision.
                ShipStarCollision(s);
            }
            //method that calculates projectile and star collision.
            ProjectileStarCollision();
           
        }

        /// <summary>
        /// This method calculates ship and star collsion.
        /// </summary>
        /// <param name="s"></param>
        private static void ShipStarCollision(Ship s)
        {

            foreach (Star star in world.starCollection.Values)
            {
                //if star location minus ship location length is less that 35, we set 
                //ships hp to 0.
                if ((star.getStarLoc() - s.getShipLoc()).Length() < 35)
                {
                    s.setShipHp(0);
                }
            }

        }

        /// <summary>
        /// This method calculates Projectile and star collision.
        /// </summary>
        private static void ProjectileStarCollision()
        {
           
            foreach (Projectile p in world.projectileCollection.Values.ToList())
            {
                foreach (Star star in world.starCollection.Values)
                {
                    //if projectile location minus star location is less than 35,set projectile alive to false.
                    if ((p.getProjectileLoc() - star.getStarLoc()).Length() < 35)
                    {
                        p.setProjectileAlive(false);

                    }
                }
            }
        }

        /// <summary>
        /// This method calculates Projectile and ship collision.
        /// </summary>
        /// <param name="s"></param>
        private static void ProjectileShipCollision(Ship s)
        {
            foreach (Projectile p in world.projectileCollection.Values.ToList())
            {
                //if ship hp is not equal to zero.
                if (s.getShipHp() != 0)
                {
                    //if projectile location and ship location is less than 25 and projectile is alive.
                    if ((p.getProjectileLoc() - s.getShipLoc()).Length() < 25 && (p.getProjectileOwner() != s.getShipId()))
                    {
                        //get the score and hp for the owner of the projectile.
                        int score = world.shipCollection[p.getProjectileOwner()].getShipScore();
                        int hp = world.shipCollection[p.getProjectileOwner()].getShipHp();

                        //decrement ships hp by 1.
                        s.setShipHp(s.getShipHp() - 1);
                        //for extra game mode.
                        if (gameMode == 1)
                        {   
                            //if projectile owner hp is less than 5.
                            if (world.shipCollection[p.getProjectileOwner()].getShipHp() < 5)
                            {
                                //icrement the ships hp by 1, if it hits other ship.
                                world.shipCollection[p.getProjectileOwner()].setShipHp((s.getShipHp() + 1));
                            }
                        }
                        p.setProjectileAlive(false);
                        //id ship hp is zer0.
                        if (s.getShipHp() == 0)
                        {
                            //extra game mode.
                            if (gameMode == 1)
                            {
                                if (hp < 5)
                                {
                                    //increments hp by 1.
                                    world.shipCollection[p.getProjectileOwner()].setShipHp(hp + 1);
                                }
                            }
                            //increments score by 1.
                            world.shipCollection[p.getProjectileOwner()].setShipScore(score + 1);

                        }
                    }
                }
            }
        }

        /// <summary>
        /// This method is used to move ship/
        /// </summary>
        /// <param name="s"></param>
        private static void moveShip(Ship s)
        {
            //calls the method shipFuntion from world class.
            s.shipFunctions();
            //sets gravity to 0,0.
            Vector2D g = new Vector2D(0, 0);
            //calculate g.
            g = star.getStarLoc() - s.getShipLoc();
            g.Normalize();
            //gets the mass of the star from xml file.
            g = g * star.getStarMass();
           
            Vector2D a = new Vector2D(0, 0);
            //the ship is thrusting.
            if (s.getShipThrust())
            {
                Vector2D t = new Vector2D(s.getShipDir());
                //calculate the thrust and apply to gravity.
                t = t * 0.08;
                a = g + t;
            }
            else
            {
                //else acceleration is equal to gravity.
                a = g;
            }
            //set thr ships velocity and location.
            s.setShipVelocity(s.getShipVelocity()  + a);
            s.setShipLoc(s.getShipLoc() + s.getShipVelocity());
        }
    }
}
