using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace TowerDefense
{
    public abstract class Tower : GameObject
    {
        // Field
        private Stopwatch stopWatch;
        private int speed;
        private int damage;
        private int cost;
        private float ranged;
        // Property
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Damage
        {
            get { return damage; }
        }
        /// <summary>
        /// The Constructor For The Tower Class
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="cost"></param>
        /// <param name="ranged"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        /// <param name="isClickAble"></param>
        public Tower(int speed, int damage, int cost, float ranged, string imagePath, PointF position, bool isClickAble) : base(imagePath, position, isClickAble)
        {
            this.speed = speed;
            this.damage = damage;
            this.cost = cost;
            this.ranged = ranged;
        }
        /// <summary>
        /// The Update function for the towers
        /// </summary>
        /// <param name="FPS"></param>
        public virtual void Update(float FPS) 
        {

        }

    }
}
