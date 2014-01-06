using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public abstract class TowerType : Tower
    {
        // test

        /// <summary>
        /// Constuctor For TowerTypes
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="cost"></param>
        /// <param name="range"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        /// <param name="isClickAble"></param>
        public TowerType(float speed, int cost, float ranged, Projectile bullet, string imagePath, PointF position, bool isClickAble) 
            : base(speed, cost, ranged, bullet, imagePath, position, isClickAble)
        {

        }
    }
}
