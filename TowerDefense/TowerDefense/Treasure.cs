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
        public Treasure(string imagePath, PointF position, bool isClickable) : base(imagePath, position, isClickable)
        {
        }
    }
}
