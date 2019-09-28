
using Newtonsoft.Json;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Model
{
    /// <summary>
    /// This class holds the JSON serialization and getters/setters for the ship game object.
    /// This class holds the JSON serialization and getters/setters for the projectile game object. 
    /// @Author: Ajay Bagali, Ritesh Sharma
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Ship
    {
        private Vector2D velocity;
        [JsonProperty(PropertyName = "ship")]
        private int ID;

        [JsonProperty(PropertyName = "loc")]
        private Vector2D loc;

        [JsonProperty(PropertyName = "dir")]
        private Vector2D dir;

        [JsonProperty(PropertyName = "thrust")]
        private bool thrust;

        [JsonProperty(PropertyName = "name")]
        private string name;

        [JsonProperty(PropertyName = "hp")]
        private int hp;

        [JsonProperty(PropertyName = "score")]

        //storing the score for each individual ship
        private int score;

        //boolean operations for the ship controls
        private bool operateLeft;
        private bool operateRight;
        //boolean for the thusting control
        private bool operateThrust;
        //field for the ship respawn counter 
        private int respawnCounter = 0;
        //field for the cooldown of the projectiles
        private int cooldownProj = 0;
        //boolean for ship firing state
        private bool isFiring = false;
        //checks if the ship's connection to the server
        private bool Disconnected;


        /// <summary>
        /// setter for the ship's firing state
        /// </summary>
        /// <param name="isFired"></param>
        public void justFired(bool isFired)
        {
            this.isFiring = isFired;
        }
        /// <summary>
        /// returns the current firing state of the ship
        /// </summary>
        /// <returns></returns>
        public bool getJustFired()
        {
            return this.isFiring;
        }

        /// <summary>
        /// this is a getter for the ship's current respawn timer
        /// </summary>
        /// <returns></returns>
        public int getCooldownProj()
        {
            return this.cooldownProj;
        }
        /// <summary>
        /// this method resets the cooldown on the firing projectiles
        /// </summary>
        public void resetCooldownProj()
        {
            this.cooldownProj = 0;
        }

        /// <summary>
        /// this method increments the projectile cooldown timer each frame
        /// </summary>
        public void incrementCooldownProj()
        {
            Interlocked.Increment(ref cooldownProj);
        }
        /// <summary>
        /// this method increments the respawn timer each frame
        /// </summary>
        public void incrementRespawnCounter()
        {
            Interlocked.Increment(ref respawnCounter);

        }
        /// <summary>
        /// this method returns the current respawn counter of the ship
        /// </summary>
        /// <returns></returns>
        public int getRespawnCounter()
        {
            return respawnCounter;
        }
        /// <summary>
        /// this retuns the ship ID 
        /// </summary>
        /// <returns></returns>
        public int getShipId()
        {
            return ID;
        }
        /// <summary>
        /// this retuns the ship location 
        /// </summary>
        /// <returns></returns>
        public Vector2D getShipLoc()
        {
            return loc;
        }
        /// <summary>
        /// this retuns the ship direction 
        /// </summary>
        /// <returns></returns>
        public Vector2D getShipDir()
        {
            return dir;
        }
        /// <summary>
        /// this retuns the boolean if the ship is thrusting or not
        /// </summary>
        /// <returns></returns>
        public bool getShipThrust()
        {
            return thrust;
        }
        /// <summary>
        /// this retuns the ship name 
        /// </summary>
        /// <returns></returns>
        public string getShipName()
        {
            return name;
        }
        /// <summary>
        /// this retuns the ship hp 
        /// </summary>
        /// <returns></returns>
        public int getShipHp()
        {
            return hp;
        }
        /// <summary>
        /// this retuns the ship score 
        /// </summary>
        /// <returns></returns>
        public int getShipScore()
        {
            return score;
        }

        /// <summary>
        /// this method sets the ship's name
        /// </summary>
        /// <param name="name"></param>
        public void setShipName(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// this method sets the unique Id to the ship
        /// </summary>
        /// <param name="id"></param>
        public void setShipID(int id)
        {
            this.ID = id;
        }
        /// <summary>
        /// this method sets the thrusting flag for the ship
        /// </summary>
        /// <param name="thrust"></param>
        public void setShipThrust(bool thrust)
        {
            this.thrust = thrust;
        }

        /// <summary>
        /// this method sets the location of the ship
        /// </summary>
        /// <param name="loc"></param>
        public void setShipLoc(Vector2D loc)
        {
            this.loc = loc;
        }
        /// <summary>
        /// this method sets the direciton of the ship
        /// </summary>
        /// <param name="dir"></param>
        public void setShipDir(Vector2D dir)
        {
            this.dir = dir;
        }
        /// <summary>
        /// this method sets the health of the current ship
        /// </summary>
        /// <param name="hp"></param>
        public void setShipHp(int hp)
        {
            this.hp = hp;
        }
        /// <summary>
        /// this method sets the score of the current ship
        /// </summary>
        /// <param name="score"></param>
        public void setShipScore(int score)
        {
            this.score = score;
        }
        /// <summary>
        /// this method is used to help with the json serialzation
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// this method sets the velocity of the ship
        /// </summary>
        /// <param name="vel"></param>
        public void setShipVelocity(Vector2D vel)
        {
            this.velocity = vel;
        }
        /// <summary>
        /// this method returns the ship's current velocity vector
        /// </summary>
        /// <returns></returns>
        public Vector2D getShipVelocity()
        {
            return this.velocity;
        }

        /// <summary>
        /// this method takes in keyboard input and sets the control flags for the ship
        /// </summary>
        /// <param name="temp"></param>
        public void doOperateShip(char temp)

        {
            if (this.hp != 0)
            {
                //if the ship turns left
                if (temp == 'L')
                {
                    this.operateLeft = true;
                }
                //if the ship turns right
                if (temp == 'R')
                {
                    this.operateRight = true;
                }
                //if the ship is thrusting
                if (temp == 'T')
                {
                    this.operateThrust = true;
                }
            }

        }
        /// <summary>
        /// this method resets the boolean control flags and changes its direction
        /// </summary>
        public void shipFunctions()
        {
            //rotating left
            if (this.operateLeft == true)
            {
                this.dir.Rotate(-2);
            }
            //resseting the left flag
            this.operateLeft = false;
            //rotating right
            if (this.operateRight == true)
            {
                this.dir.Rotate(2);
            }
            //resetting the right flag
            this.operateRight = false;
            //ship thursting
            if (this.operateThrust == true)
            {
                this.setShipThrust(true);
            }
            //resetting the thrust boolean
            this.operateThrust = false;

        }
        /// <summary>
        /// this sets the thrusting boolean for the ship
        /// </summary>
        /// <param name="operaL"></param>
        public void setOperateLeft(bool operaL)
        {
            this.operateLeft = operaL;
        }
        /// <summary>
        /// this sets the right control boolean
        /// </summary>
        /// <param name="operaR"></param>
        public void setOperateRight(bool operaR)
        {
            this.operateRight = operaR;
        }
        /// <summary>
        /// this sets the left control boolean
        /// </summary>
        /// <returns></returns>
        public bool getOperateLeft()
        {
            return this.operateLeft;

        }
        /// <summary>
        /// this returns the ship's right command boolean flag
        /// </summary>
        /// <returns></returns>
        public bool getOperateRight()
        {
            return this.operateRight;
        }
        /// <summary>
        /// this returns the ship's thrusting command boolean flag
        /// </summary>
        /// <returns></returns>
        public bool getOperateThrust()
        {
            return this.operateThrust;
        }



        /// <summary>
        /// this method sets the connection state of the ship
        /// </summary>
        /// <param name="disconnect"></param>
        public void setDisconnected(bool disconnect)
        {
            this.Disconnected = disconnect;
        }
        /// <summary>
        /// this method returns the connection state of the ship 
        /// </summary>
        /// <returns></returns>
        public bool getDisconnected()
        {
            return this.Disconnected;
        }

        /// <summary>
        /// this method handles the respawing of dead ships
        /// </summary>
        /// <param name="loc"></param>
        public void respawn(Vector2D loc)
        {
            //resetting the old ship's fields
            this.setShipLoc(loc);
            this.setShipHp(5);
            this.setShipDir(new Vector2D(0, -1));
            this.setShipVelocity(new Vector2D(0, 0));
            //resetting its respawn counter back down to 0
            Interlocked.Exchange(ref respawnCounter, 0);

        }


    }


}
