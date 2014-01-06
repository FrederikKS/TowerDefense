using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public class TowerStun : TowerType
    {
        // test

        // Fields
        private float stunTime;
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
        public TowerStun(float stunTime,float speed, int cost, float ranged, Projectile bullet, string imagePath, PointF position, bool isClickAble)
            : base(speed, cost, ranged, bullet, imagePath, position, isClickAble)
        {

        }
    }
}
