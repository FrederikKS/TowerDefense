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
        public TowerStun(float stunTime,float speed, int damage, int cost, float range, string imagePath, PointF position, bool isClickAble)
            : base(speed, damage, cost, range, imagePath, position, isClickAble)
        {

        }
    }
}
