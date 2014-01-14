using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{    
    public abstract class Environment : GameObject
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        public Environment(string imagePath, PointF position) : base(imagePath, position)
        {
            
        }
    }
}
