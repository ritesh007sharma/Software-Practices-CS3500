
using Model;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DrawingPanel
{
    /// <summary>
    /// This class holds the drawing panel for the space wars game. 
    /// It draws the world and its objects that will be later displayed on the form. 
    /// </summary>
    public class DrawingPanel : Panel
    {
        //holds the instance of the world it draws from 
        private World theWorld;
        //model dictionaries to hold local images which have already stored the local file path
        private Dictionary<string, Image> shipDictionary;
        private Dictionary<string, Image> projDictionary;
        //local field to draw the star in the game
        private Image onlyStar;
        //this image holds the game space image background
        private Image spaceBackground;

        /// <summary>
        /// this constrcutor for the drawing panel class defines the images used in the game as well the world instance 
        /// </summary>
        /// <param name="world"></param>
        public DrawingPanel(World world)
        {
            //reduces the flickering
            DoubleBuffered = true;
            theWorld = world;
            //storing the star and the background locally for reuse, rather than getting the image by finding the file each time 
            onlyStar = Image.FromFile("../../../Resources/Images/star.jpg");
            spaceBackground = Image.FromFile("../../../Resources/Images/backgroun.png");

            shipDictionary = new Dictionary<string, Image>();
            projDictionary = new Dictionary<string, Image>();

            //putting local ship  images into a dictionary to prevent from rereading file addresses to get the image
            shipDictionary.Add("blueThrust", Image.FromFile("../../../Resources/Images/ship-thrust-blue.png"));
            shipDictionary.Add("blueCoast", Image.FromFile("../../../Resources/Images/ship-coast-blue.png"));
            shipDictionary.Add("brownThrust", Image.FromFile("../../../Resources/Images/ship-thrust-brown.png"));
            shipDictionary.Add("brownCoast", Image.FromFile("../../../Resources/Images/ship-coast-brown.png"));
            shipDictionary.Add("greenThrust", Image.FromFile("../../../Resources/Images/ship-thrust-green.png"));
            shipDictionary.Add("greenCoast", Image.FromFile("../../../Resources/Images/ship-coast-green.png"));
            shipDictionary.Add("greyThrust", Image.FromFile("../../../Resources/Images/ship-thrust-grey.png"));
            shipDictionary.Add("greyCoast", Image.FromFile("../../../Resources/Images/ship-coast-grey.png"));
            shipDictionary.Add("redThrust", Image.FromFile("../../../Resources/Images/ship-thrust-red.png"));
            shipDictionary.Add("redCoast", Image.FromFile("../../../Resources/Images/ship-coast-red.png"));
            shipDictionary.Add("voiletThrust", Image.FromFile("../../../Resources/Images/ship-thrust-violet.png"));
            shipDictionary.Add("voiletCoast", Image.FromFile("../../../Resources/Images/ship-coast-violet.png"));
            shipDictionary.Add("whiteThrust", Image.FromFile("../../../Resources/Images/ship-thrust-white.png"));
            shipDictionary.Add("whiteCoast", Image.FromFile("../../../Resources/Images/ship-coast-white.png"));
            shipDictionary.Add("yellowThrust", Image.FromFile("../../../Resources/Images/ship-thrust-yellow.png"));
            shipDictionary.Add("yellowCoast", Image.FromFile("../../../Resources/Images/ship-coast-yellow.png"));

            //putting local projectile  images into a dictionary to prevent from rereading file addresses to get the image
            projDictionary.Add("pBlue", Image.FromFile("../../../Resources/Images/shot-blue.png"));
            projDictionary.Add("pBrown", Image.FromFile("../../../Resources/Images/shot-brown.png"));
            projDictionary.Add("pGreen", Image.FromFile("../../../Resources/Images/shot-green.png"));
            projDictionary.Add("pGrey", Image.FromFile("../../../Resources/Images/shot-grey.png"));
            projDictionary.Add("pRed", Image.FromFile("../../../Resources/Images/shot-red.png"));
            projDictionary.Add("pViolet", Image.FromFile("../../../Resources/Images/shot-violet.png"));
            projDictionary.Add("pWhite", Image.FromFile("../../../Resources/Images/shot-white.png"));
            projDictionary.Add("pYellow", Image.FromFile("../../../Resources/Images/shot-yellow.png"));




        }
        /// <summary>
        /// Helper method for DrawObjectWithTransform
        /// </summary>
        /// <param name="size">The world (and image) size</param>
        /// <param name="w">The worldspace coordinate</param>
        /// <returns></returns>
        private static int WorldSpaceToImageSpace(int size, double w)
        {
            return (int)w + size / 2;
        }



        // A delegate for DrawObjectWithTransform
        // Methods matching this delegate can draw whatever they want using e  
        public delegate void ObjectDrawer(object o, PaintEventArgs e);

        /// <summary>
        /// This method performs a translation and rotation to drawn an object in the world.
        /// </summary>
        /// <param name="e">PaintEventArgs to access the graphics (for drawing)</param>
        /// <param name="o">The object to draw</param>
        /// <param name="worldSize">The size of one edge of the world (assuming the world is square)</param>
        /// <param name="worldX">The X coordinate of the object in world space</param>
        /// <param name="worldY">The Y coordinate of the object in world space</param>
        /// <param name="angle">The orientation of the objec, measured in degrees clockwise from "up"</param>
        /// <param name="drawer">The drawer delegate. After the transformation is applied, the delegate is invoked to draw whatever it wants</param>
        private void DrawObjectWithTransform(PaintEventArgs e, object o, int worldSize, double worldX, double worldY, double angle, ObjectDrawer drawer)
        {
            // Perform the transformation
            int x = WorldSpaceToImageSpace(worldSize, worldX);
            int y = WorldSpaceToImageSpace(worldSize, worldY);

            e.Graphics.TranslateTransform(x, y);
            e.Graphics.RotateTransform((float)angle);
            // Draw the object 
            drawer(o, e);
            // Then undo the transformation
            e.Graphics.ResetTransform();

        }



        /// <summary>
        /// This method draws the ships using the object drawer delegate and determines the color of each of the ships.  
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ShipDrawer(object o, PaintEventArgs e)
        {
            //setting up the ship drawing object
            Ship s = o as Ship;
            //we set up an image
            Image image = null;
            //we define the image dimentions for when they were drawn
            int shipwidth = 35;

            //this returns the image with the proper ship color based on the ship ID
            image = determineShipColor(s, image);


            //this draws the ship object on the panel, given its specific image and location 
            e.Graphics.DrawImage(image, 0 - (shipwidth / 2), 0 - (shipwidth / 2), shipwidth, shipwidth);




        }


        /// <summary>
        /// This method draws the projectiles using the object drawer delegate and determines the color of each of the projectiles.  
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void ProjectileDrawer(object o, PaintEventArgs e)
        {
            //setting up the projectile as a drawing reference
            Projectile p = o as Projectile;
            //setting up an image
            Image projecitleImage = null;
            //this helper method determines the color of the projectile and gives it back as an image
            projecitleImage = determineProjectileColor(p, projecitleImage);
            //size of the projectile 
            int projectileWidth = 25;
            //this draws the projectile object on the panel, given its specific image and location 
            e.Graphics.DrawImage(projecitleImage, 0 - (projectileWidth / 2), 0 - (projectileWidth / 2), projectileWidth, projectileWidth);


        }
        /// <summary>
        /// This method draws the star using the object drawer delegate 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void StarDrawer(object o, PaintEventArgs e)
        {
            //setting up the star as a drawing reference
            Star s = o as Star;
            //getting the star image from our private field
            Image image = onlyStar;
            //size of the star
            int starWidth = 35;

            //this draws the star object on the panel, given its specific image and location 
            e.Graphics.DrawImage(image, 0 - (starWidth / 2), 0 - (starWidth / 2), 35, 35);
        }

        /// <summary>
        /// This method is invoked when the DrawingPanel needs to be re-drawn, iterates through the world dictionaries, and draws the panel with the game objects on the panel.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //drawing the world background/space background
            int size = theWorld.worldSize;
            e.Graphics.DrawImage(spaceBackground, 0, 0, size, size);

            //locking the world here because we update the world internally and when we update it on the form
            lock (theWorld)
            {
                //iterates through the ship dictionary in the world, to draw each of the ships
                foreach (KeyValuePair<int, Ship> ship in theWorld.shipCollection)
                {
                    //only drawing alive ships
                    if (ship.Value.getShipHp() > 0)
                    {
                        DrawObjectWithTransform(e, ship.Value, theWorld.worldSize, ship.Value.getShipLoc().GetX(), ship.Value.getShipLoc().GetY(), ship.Value.getShipDir().ToAngle(), ShipDrawer);
                    }
                }
                //draws the star in the star dictionary, even though there is only one star
                foreach (KeyValuePair<int, Star> star in theWorld.starCollection)
                {

                    DrawObjectWithTransform(e, star.Value, theWorld.worldSize, star.Value.getStarLoc().GetX(), star.Value.getStarLoc().GetY(), 0, StarDrawer);
                }
                //   //iterates through the projectile dictionary in the world, to draw each of the projectiles
                foreach (KeyValuePair<int, Projectile> projectile in theWorld.projectileCollection)
                {

                    DrawObjectWithTransform(e, projectile.Value, theWorld.worldSize, projectile.Value.getProjectileLoc().GetX(), projectile.Value.getProjectileLoc().GetY(), projectile.Value.getProjectileDir().ToAngle(), ProjectileDrawer);
                }

            }

            //  Do anything that Panel (from which we inherit) needs to do on an rectangle, in this case our scoreboard
            DrawFillRectangleInt(e);
            //this drwas the models using its paint method
            base.OnPaint(e);
        }

        /// <summary>
        /// this method draws our scoreboard
        /// </summary>
        /// <param name="e"></param>
        public void DrawFillRectangleInt(PaintEventArgs e)
        {
            //make a sorted list by the ship score, using our own comparator method in our world class
            SortedList<int, Ship> sortedShipScore =
               new SortedList<int, Ship>(theWorld.shipCollection, new Model.scoreComparer(theWorld));
            //this draws the health bars of each of the ships
            drawPanel(sortedShipScore, e);

        }
        /// <summary>
        /// This method draws the rectangles in our scoreboard on the panel, and utilizes the comparator to rank the ship scores. 
        /// </summary>
        /// <param name="sorted"></param>
        /// <param name="e"></param>
        public void drawPanel(SortedList<int, Ship> sorted, PaintEventArgs e)
        {
            //pen to draw the outline rectangle
            Pen p = new Pen(Brushes.Black);
            //our height offset
            int heightOffset = 15;
            //our length offset
            int lengthOffset = theWorld.worldSize + 25;

            //drwas the white panel in the background
            e.Graphics.FillRectangle(Brushes.White, theWorld.worldSize, 0, this.Size.Width - theWorld.worldSize, this.Size.Height);

            //iterating through our ship collection
            foreach (Ship ship in sorted.Values)
            {
                //the ship HP
                double hp = ship.getShipHp();

                //this draws the text above the health bars
                e.Graphics.DrawString(ship.getShipName() + ": " + ship.getShipScore(), new Font("Areil", 8), Brushes.Black, lengthOffset, heightOffset - 15);

                //this draws the inner rectangle inside the white rectangle
                e.Graphics.DrawRectangle(p, lengthOffset, heightOffset, 200, 15);

                //we set the color to green if the ship has a high health
                if (hp >= 4)
                {
                    e.Graphics.FillRectangle(Brushes.Green, lengthOffset, heightOffset + 1, (int)(200 * (hp / 5)), 15);
                }
                //if the ship hp is in the middle, we draw it orange
                else if (hp >= 2 && hp <= 4)
                {
                    e.Graphics.FillRectangle(Brushes.Orange, lengthOffset, heightOffset + 1, (int)(200 * (hp / 5)), 15);
                }
                //if the ship hp is low, we make it red
                else
                {
                    e.Graphics.FillRectangle(Brushes.Red, lengthOffset, heightOffset + 1, (int)(200 * (hp / 5)), 15);
                }

                heightOffset = heightOffset + 40;
            }
        }

        /// <summary>
        /// This method takes the ship Id and matches a certain ship color with it and returns that specific ship image
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="shipImage"></param>
        /// <returns></returns>
        private Image determineShipColor(Ship ship, Image shipImage)
        {

            //for all of these ships, we mod the ship ID by 8 because there are 8 colors, to simply identify a certain ship color associated with the ID
            //To determine if it is thrusting or if it coasting, we simply check each ship's boolean thrusting field to choose which image we need 
            if (ship.getShipId() % 8 == 0)
            {
                if (ship.getShipThrust() == true)
                {
                    shipImage = shipDictionary["blueThrust"];
                }
                else
                {
                    shipImage = shipDictionary["blueCoast"];
                }

            }
            if (ship.getShipId() % 8 == 1)
            {
                if (ship.getShipThrust() == true)
                {
                    shipImage = shipDictionary["brownThrust"];
                }
                else
                {
                    shipImage = shipDictionary["brownCoast"];
                }
            }
            if (ship.getShipId() % 8 == 2)
            {
                if (ship.getShipThrust() == true)
                {
                    shipImage = shipDictionary["greenThrust"];

                }
                else
                {
                    shipImage = shipDictionary["greenCoast"];
                }
            }
            if (ship.getShipId() % 8 == 3)
            {
                if (ship.getShipThrust() == true)
                {
                    shipImage = shipDictionary["greyThrust"];

                }
                else
                {
                    shipImage = shipDictionary["greyCoast"];
                }
            }
            if (ship.getShipId() % 8 == 4)
            {
                if (ship.getShipThrust() == true)
                {
                    shipImage = shipDictionary["redThrust"];

                }
                else
                {
                    shipImage = shipDictionary["redCoast"];
                }
            }
            if (ship.getShipId() % 8 == 5)
            {
                if (ship.getShipThrust() == true)
                {
                    shipImage = shipDictionary["voiletThrust"];

                }
                else
                {
                    shipImage = shipDictionary["voiletCoast"];
                }
            }
            if (ship.getShipId() % 8 == 6)
            {
                if (ship.getShipThrust() == true)
                {
                    shipImage = shipDictionary["whiteThrust"];

                }
                else
                {
                    shipImage = shipDictionary["whiteCoast"];
                }
            }
            if (ship.getShipId() % 8 == 7)
            {
                if (ship.getShipThrust() == true)
                {
                    shipImage = shipDictionary["yellowThrust"];

                }
                else
                {
                    shipImage = shipDictionary["yellowCoast"];
                }
            }
            //we return that specific image that matches with the ship ID that was passed in
            return shipImage;
        }

        /// <summary>
        /// This method takes the projectile and matches a certain projectile color with it and returns that specific projectile image
        /// </summary>
        /// <param name="ship"></param>
        /// <param name="shipImage"></param>
        /// <returns></returns>
        private Image determineProjectileColor(Projectile projectile, Image projectileImage)
        {

            //for all of these projectiles, we first find the projectile ownwe and we match it the same way we find the ship color 
            if (projectile.getProjectileOwner() % 8 == 0)
            {
                projectileImage = projDictionary["pBlue"];
            }
            if (projectile.getProjectileOwner() % 8 == 1)
            {
                projectileImage = projDictionary["pBrown"];
            }
            if (projectile.getProjectileOwner() % 8 == 2)
            {
                projectileImage = projDictionary["pGreen"];
            }
            if (projectile.getProjectileOwner() % 8 == 3)
            {
                projectileImage = projDictionary["pGrey"];
            }
            if (projectile.getProjectileOwner() % 8 == 4)
            {
                projectileImage = projDictionary["pRed"];
            }
            if (projectile.getProjectileOwner() % 8 == 5)
            {
                projectileImage = projDictionary["pViolet"];
            }
            if (projectile.getProjectileOwner() % 8 == 6)
            {
                projectileImage = projDictionary["pWhite"];
            }
            if (projectile.getProjectileOwner() % 8 == 7)
            {
                projectileImage = projDictionary["pYellow"];
            }
            //we return that specific image that matches with the projectile owner that was passed in
            return projectileImage;
        }
    }
}