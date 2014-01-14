using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    class Treasure : Environment
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        public Treasure(string imagePath, PointF position) : base(imagePath, position)
        {
        }
    }
}
