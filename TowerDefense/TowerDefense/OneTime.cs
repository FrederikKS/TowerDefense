using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public class OneTime : Tower
    {
        // test

        /// <summary>
        /// Constructor For The OneTime Use Towers
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="cost"></param>
        /// <param name="ranged"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        /// <param name="isClickAble"></param>
        public OneTime(float speed, int damage, int cost, float ranged, string imagePath, PointF position, bool isClickAble) 
            : base(speed,damage,cost, ranged, imagePath,position,isClickAble)
        {

        }
    }
}
