using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace TowerDefense
{
    public class TowerStun : Tower
    {
        // Fields
        private float stunTime = 0;
        // Property
        public float StunTime
        {
            get {return stunTime; }
        }
        /// <summary>
        /// The Constructor For The TowerStun
        /// </summary>
        /// <param name="stunTime"></param>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="cost"></param>
        /// <param name="range"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        /// <param name="isClickAble"></param>
        public TowerStun(float stunTime,float speed, int cost, float ranged, string imagePath, PointF position)
            : base(speed, cost, ranged, imagePath, position)
        {
            this.Bullet = new Projectile(10, 5, @"Towers/w2.png", position, this);
        }
    }
}
