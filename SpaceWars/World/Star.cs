
using Newtonsoft.Json;
using SpaceWars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    /// <summary>
    /// This class holds the JSON serialization and getters/setters for the ship star object. 
    /// @Author: Ritesh Sharma, Ajay Bagali
    /// 12/8/18 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Star
    {
        [JsonProperty(PropertyName = "star")]
        private int ID;

        [JsonProperty(PropertyName = "loc")]
        private Vector2D loc;

        [JsonProperty(PropertyName = "mass")]
        //field for the mass of the star
        private double mass;
        /// <summary>
        /// this constructor just sets the fields for each new star existing in the star collection
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="loc"></param>
        /// <param name="mass"></param>
        public Star(int ID, Vector2D loc, double mass)
        {
            this.ID = ID;
            this.loc = loc;
            this.mass = mass;
        }
        /// <summary>
        /// this method returns the star ID
        /// </summary>
        /// <returns></returns>
        public int getStarId()
        {
            return ID;
        }
        /// <summary>
        /// this method returns the star location
        /// </summary>
        /// <returns></returns>
        public Vector2D getStarLoc()
        {
            return loc;
        }
        /// <summary>
        /// this sets the star location in the world
        /// </summary>
        /// <param name="loc"></param>
        public void setStarLoc(Vector2D loc)
        {
            this.loc = loc;
        }
        /// <summary>
        /// this method returns the mass of the star 
        /// </summary>
        /// <returns></returns>
        public double getStarMass()
        {
            return mass;
        }
        /// <summary>
        /// this overrides the to string to JSon serialization
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}