using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    class Checkpoint : Environment
    {
        // Constructor for Checkpoint
        public Checkpoint(string imagePath, PointF position) : base(imagePath, position)
        {
            
        }
    }
}
