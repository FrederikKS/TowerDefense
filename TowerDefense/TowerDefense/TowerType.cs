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
        public TowerType(int speed, int damage, int cost, float range, string imagePath, PointF position, bool isClickAble) : base(speed, damage, cost, range, imagePath, position, isClickAble)
        {

        }
    }
}
