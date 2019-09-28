using Newtonsoft.Json;
using SpaceWars;


namespace Model
{
    /// <summary>
    /// This class holds the JSON serialization and getters/setters for the projectile game object. 
    /// @Author: Ritesh Sharma, Ajay Bagali
    /// 12/8/18 
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class Projectile
    {

        private Vector2D velocity;
        [JsonProperty(PropertyName = "proj")]
        private int ID;

        [JsonProperty(PropertyName = "loc")]
        private Vector2D loc;

        [JsonProperty(PropertyName = "dir")]
        private Vector2D dir;

        [JsonProperty(PropertyName = "alive")]
        private bool alive;

        [JsonProperty(PropertyName = "owner")]
        private int owner;

        /// <summary>
        /// this method sets the unique Id for the projectile
        /// </summary>
        /// <param name="ID"></param>
        public void setProjectileID(int ID)
        {
            this.ID = ID;
        }
        /// <summary>
        /// this method sets the location of the projectile
        /// </summary>
        /// <param name="loc"></param>
        public void setProjectileLoc(Vector2D loc)
        {
            this.loc = loc;
        }
        /// <summary>
        /// this method sets the direction of the projecitle
        /// </summary>
        /// <param name="dir"></param>
        public void setProjectileDir(Vector2D dir)
        {
            this.dir = dir;
        }
        /// <summary>
        /// this method sets the alive or not factor for the projectile
        /// </summary>
        /// <param name="alive"></param>
        public void setProjectileAlive(bool alive)
        {
            this.alive = alive;
        }
        /// <summary>
        /// this method sets the projectile owner
        /// </summary>
        /// <param name="owner"></param>
        public void setProjectileOwner(int owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// this method returns the projectile ID
        /// </summary>
        /// <returns></returns>
        public int getProjectileId()
        {
            return ID;
        }
        /// <summary>
        /// this method returns the projectile location
        /// </summary>
        /// <returns></returns>
        public Vector2D getProjectileLoc()
        {
            return loc;
        }
        /// <summary>
        /// this method returns the projectile direction
        /// </summary>
        /// <returns></returns>
        public Vector2D getProjectileDir()
        {
            return dir;
        }
        /// <summary>
        /// this method returns a boolean if the projectile is alive 
        /// </summary>
        /// <returns></returns>
        public bool getProjectileAlive()
        {
            return alive;
        }
        /// <summary>
        /// this method returns the projectile owner
        /// </summary>
        /// <returns></returns>
        public int getProjectileOwner()
        {
            return owner;
        }
        /// <summary>
        /// this method sets the velocity of the projectile
        /// </summary>
        /// <param name="vel"></param>
        public void setProjectileVelocity(Vector2D vel)
        {
            this.velocity = vel;
        }
        /// <summary>
        /// this method returns the current vector of the projectile velocity
        /// </summary>
        /// <returns></returns>
        public Vector2D getProjectileVelocity()
        {
            return this.velocity;
        }

        /// <summary>
        /// this method helps with the projectile json serialization 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
