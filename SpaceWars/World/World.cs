using SpaceWars;
using System;
using System.Collections.Generic;

namespace Model
{
    /// <summary>
    /// this class holds the world object for our game to use, which involes all of the game objects as well
    /// @Author: Ritesh Sharma, Ajay Bagali.
    /// 12/8/18 
    /// </summary>
    public class World
    {
        //this dictionary holds the ships in the game, represented by the ship ID
        public Dictionary<int, Ship> shipCollection;
        //this dictionary holds the projectiles in the game, represented by the projectile ID
        public Dictionary<int, Projectile> projectileCollection;
        //this dictionary holds the star in the game, represented by the star ID
        public Dictionary<int, Star> starCollection;
        //this holds the size of the world
        public int worldSize;
        //holding the id for each ship
        public static int id;
        //respawning for the star location
        public static Star star;
        //responsible for the respawing location
        private static Random rand;



        /// <summary>
        /// the world constructor holds the dictionaries and its size
        /// </summary>
        public World()
        {
            //resetting the Id each new instance of the world
            id = 0;
            shipCollection = new Dictionary<int, Ship>();
            projectileCollection = new Dictionary<int, Projectile>();
            starCollection = new Dictionary<int, Star>();
            worldSize = 0;
            rand = new Random();

        }

        /// <summary>
        /// returns the world's ship collection
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, Ship> getShipCollection()
        {
            return shipCollection;
        }

        /// <summary>
        /// generates new incrementing id for the ships
        /// </summary>
        /// <returns></returns>
        public int generateID()
        {
            id++;
            return id;
        }

        /// <summary>
        /// this method makes a new ship for a new client connection
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Ship generatenewShip(string name)
        {
            //assigning a unique ship ID
            int id = generateID();

            //setting up the default fields of the ship including its name and id
            Ship ship = new Ship();
            ship.setShipVelocity(new Vector2D(0, 0));
            ship.setShipName(name);
            ship.setShipID(id);

            //generating a random spawn location in the world
            Vector2D safeLocation = generateLocation(ship);

            //setting its new location
            ship.setShipLoc(safeLocation);
            //resseting its direciton to default
            ship.setShipDir(new Vector2D(0, -1));
            //giving it a default thrusting flag
            ship.setShipThrust(false);
            ship.setShipHp(5);

            //adding the new ship with its unique id to the ship colleciton
            shipCollection.Add(ship.getShipId(), ship);

            return ship;
        }




        /// <summary>
        /// this method gives generates a proper random ship spawn location
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public Vector2D generateLocation(Ship s)
        {
            //generatoring a random vector assigned within the world size
            Vector2D safeLocation = new Vector2D(rand.NextDouble() * (750 / 2), rand.NextDouble() * (750 / 2));

            //setting the defualt star size
            double starSize = 35;

            //this makes sure that that new spawn location doesn't collide with the location of the star
            while (safeLocation.GetX() < starSize && safeLocation.GetY() < starSize)
            {

                double x = rand.NextDouble();
                double y = rand.NextDouble();
                x = x * (750 / 2);
                y = y * (750 / 2);
                //this makes the x and the y variabes into the new ship location vector
                safeLocation = new Vector2D(x, y);
            }

            return safeLocation;
        }

        public Dictionary<int, Projectile> getProjectileDictionary()
        {
            return projectileCollection;
        }
    }
    /// <summary>
    /// this comparator is used to compare the ship scores in the ship dictionary for when it is drawn in the scoreboard
    /// </summary>
    public class scoreComparer : IComparer<int>
    {
        //instance of the world object
        private World theWorld;

        /// <summary>
        /// this constructor takes in the instance of the world object to stay consistent
        /// </summary>
        /// <param name="world"></param>
        public scoreComparer(World world)
        {
            this.theWorld = world;
        }
        /// <summary>
        /// this comparator takes two ints and returns the larger one
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public int Compare(int x, int y)
        {
            //this represents one ship
            Ship playerOne = theWorld.shipCollection[x];
            //this represents another ship
            Ship playerTwo = theWorld.shipCollection[y];

            //returns the higher score
            return playerTwo.getShipScore() - playerOne.getShipScore();


        }


    }

}








