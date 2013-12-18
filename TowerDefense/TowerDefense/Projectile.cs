using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    class Projectile : Tower
    {
        //Fields
        private PointF location;

        //Properties
        public PointF Location
        {
            get { return location; }
            set { location = value; }
        }

        /// <summary>
        /// Projectile Constructor
        /// </summary>
        /// <param name="location"></param>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="cost"></param>
        /// <param name="range"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        /// <param name="isClickable"></param>
        public Projectile(PointF location, float speed, int damage, int cost, float range, string imagePath, PointF position, bool isClickable) : base(speed, damage, cost, range, imagePath, position, isClickable)
        {
            this.Location = location;
        }
    }
}
