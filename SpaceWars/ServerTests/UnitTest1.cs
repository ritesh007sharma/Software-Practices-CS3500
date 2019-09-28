using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Model;
/// <summary>
/// This testing class is used to test the Model of the space wars server
/// @Author: Ajay Bagali, Ritesh Sharma
/// 12/8/18
/// </summary>
namespace Tests
{
    [TestClass]
    public class UnitTest1
    {



        [TestMethod]
        //tests the world dicitonaries
        public void CountShips()
        {
            World theWorld = new World();
            theWorld.worldSize = 750;
            theWorld.generatenewShip("1");
            theWorld.generatenewShip("2");
            Assert.IsTrue(theWorld.getShipCollection().Count == 2);

        }
        [TestMethod]
        //tests the ships setup in the world
        public void ShipID()
        {
            World theWorld = new World();
            theWorld.worldSize = 750;
            theWorld.generatenewShip("3");
            theWorld.generatenewShip("4");
            Dictionary<int, Ship> sc = new Dictionary<int, Ship>();
            sc = theWorld.getShipCollection();
            //checking name against the id
            Assert.AreEqual(1, sc[1].getShipId());
            Assert.IsTrue(2 == sc[2].getShipId());
        }

        [TestMethod]
        //testing the vectors/location of the ships
        public void shipLocation()
        {
            World theWorld = new World();
            theWorld.worldSize = 750;
            theWorld.shipCollection.Add(0, new Ship());
            theWorld.shipCollection.Add(1, new Ship());
            theWorld.shipCollection[0].setShipLoc(new SpaceWars.Vector2D(0, 0));
            theWorld.shipCollection[1].setShipLoc(new SpaceWars.Vector2D(0, 0));



            Assert.IsTrue(theWorld.shipCollection[0].getShipLoc().GetX() < theWorld.worldSize);
            Assert.IsTrue(theWorld.shipCollection[0].getShipLoc().GetY() < theWorld.worldSize);
            Assert.IsTrue(theWorld.shipCollection[1].getShipLoc().GetX() < theWorld.worldSize);
            Assert.IsTrue(theWorld.shipCollection[1].getShipLoc().GetY() < theWorld.worldSize);
            theWorld.shipCollection.Remove(0);
            theWorld.shipCollection.Remove(1);
            Assert.IsTrue(theWorld.shipCollection.Count == 0);


        }

        [TestMethod]
        //testing the set up of the star
        public void StarID()
        {
            World theWorld = new World();
            Star star = new Star(1, new SpaceWars.Vector2D(0, 0), 45);
            theWorld.starCollection.Add(0, star);
            Assert.IsTrue(theWorld.starCollection[0].getStarId() == 1);
            Assert.IsTrue(theWorld.starCollection[0].getStarLoc().GetX() == 0);
            Assert.IsTrue(theWorld.starCollection[0].getStarLoc().GetY() == 0);

            Assert.IsTrue(theWorld.starCollection[0].getStarMass() == 45);
        }

        [TestMethod]
        //testing the star collection 
        public void countStars()
        {
            World theWorld = new World();
            Star star = new Star(1, new SpaceWars.Vector2D(0, 0), 45);
            Assert.IsTrue(0 == theWorld.starCollection.Count);
        }


        [TestMethod]
        //testing the set up of our projectiles
        public void setupProjectiles()
        {
            World theWorld = new World();
            Projectile p = new Projectile();
            p.setProjectileID(1);
            Assert.IsTrue(1 == p.getProjectileId());
            p.setProjectileLoc(new SpaceWars.Vector2D(1, 1));
            Assert.IsTrue(1 == p.getProjectileLoc().GetX());
            Assert.IsTrue(1 == p.getProjectileLoc().GetY());

            p.setProjectileDir(new SpaceWars.Vector2D(0, 0));
            Assert.IsTrue(0 == p.getProjectileDir().GetX());
            Assert.IsTrue(0 == p.getProjectileDir().GetY());
            p.setProjectileAlive(true);
            Assert.IsTrue(p.getProjectileAlive());
            p.setProjectileOwner(1);
            Assert.IsTrue(1 == p.getProjectileOwner());
            Dictionary<int, Projectile> pc = theWorld.getProjectileDictionary();
            Assert.IsTrue(pc.Count == 0);
        }
        [TestMethod]
        //tesing the setup of the ships
        public void setupShips()
        {
            World theWorld = new World();
            theWorld.generatenewShip("1");
            Dictionary<int, Ship> dc = new Dictionary<int, Ship>();
            dc = theWorld.shipCollection;
            dc[1].justFired(true);
            Assert.IsTrue(dc[1].getJustFired());
            int cd = dc[1].getCooldownProj();
            dc[1].resetCooldownProj();
            Assert.IsTrue(0 == cd);
            dc[1].incrementCooldownProj();
            Assert.IsTrue(1 == dc[1].getCooldownProj());
            dc[1].incrementRespawnCounter();
            Assert.IsTrue(1 == dc[1].getRespawnCounter());
            dc[1].setShipID(1);
            Assert.IsTrue(1 == dc[1].getShipId());
            dc[1].setShipThrust(true);
            Assert.IsTrue(dc[1].getShipThrust());
            dc[1].setShipLoc(new SpaceWars.Vector2D(0, 0));
            Assert.AreEqual(0, dc[1].getShipLoc().GetX());
            Assert.AreEqual(0, dc[1].getShipLoc().GetY());
            dc[1].setShipDir(new SpaceWars.Vector2D(0, 0));
            Assert.AreEqual(0, dc[1].getShipDir().GetX());
            Assert.AreEqual(0, dc[1].getShipDir().GetY());
            dc[1].setShipHp(4);
            Assert.AreEqual(4, dc[1].getShipHp());
            dc[1].setShipScore(3);
            Assert.AreEqual(3, dc[1].getShipScore());
            dc[1].setShipThrust(true);
            Assert.IsTrue(dc[1].getShipThrust());


        }

        [TestMethod]

        public void shipCommandsSetup()
        {
            World theWorld = new World();
            theWorld.generatenewShip("1");
            Dictionary<int, Ship> dc = new Dictionary<int, Ship>();
            dc = theWorld.shipCollection;
            dc[1].doOperateShip('L');
            Assert.IsTrue(dc[1].getOperateLeft());
            dc[1].doOperateShip('R');
            Assert.IsTrue(dc[1].getOperateRight());
            dc[1].doOperateShip('T');
            Assert.IsTrue(dc[1].getOperateThrust());


        }
        [TestMethod]
        public void ShipBasicFunctionality()
        {
            World theWorld = new World();
            theWorld.generatenewShip("1");
            Dictionary<int, Ship> dc = new Dictionary<int, Ship>();
            dc = theWorld.shipCollection;
            dc[1].setOperateRight(true);
            Assert.IsTrue(dc[1].getOperateRight());
            dc[1].setOperateLeft(true);
            Assert.IsTrue(dc[1].getOperateLeft());

        }
        [TestMethod]
        public void shipCommandsReset()
        {
            World theWorld = new World();
            theWorld.generatenewShip("1");
            Dictionary<int, Ship> dc = new Dictionary<int, Ship>();
            dc = theWorld.shipCollection;
            dc[1].doOperateShip('L');
            dc[1].doOperateShip('R');
            dc[1].doOperateShip('T');
            dc[1].shipFunctions();

            Assert.IsFalse(dc[1].getOperateLeft());

            Assert.IsFalse(dc[1].getOperateRight());

            Assert.IsFalse(dc[1].getOperateThrust());


        }
        [TestMethod]
        //testing ID's of ship objects
        public void staticWorldID()
        {
            World theWorld = new World();
            theWorld.generatenewShip("1");
            World otherWorld = new World();
            otherWorld.generatenewShip("2");
            Assert.IsTrue(theWorld.shipCollection[1].getShipId() == 1);


        }



    }
}





































