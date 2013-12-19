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
        // test

        // Field
        private Stopwatch stopWatch;
        private float speed;
        private int damage;
        private int cost;
        private float ranged;
        // Property
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Damage
        {
            get { return damage; }
        }
        public Stopwatch StopWatch
        {
            get { return stopWatch; }
            set { stopWatch = value; }
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
        public Tower(float speed, int damage, int cost, float ranged, string imagePath, PointF position, bool isClickable) : base(imagePath, position, isClickable)
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
