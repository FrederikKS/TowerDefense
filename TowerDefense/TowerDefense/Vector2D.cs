using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{
    public struct Vector2D
    {

        public float X;
        public float Y;


        public Vector2D(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        /// <summary>
        /// Normalizes vector values. This is used for defining direction in X and Y axis
        /// </summary>
        public void Normalize()
        {
            if (X < 0)
            {
                X = -1;
            }
            else
            {
                X = 1;
            }
            if (Y > 0)
            {
                Y = 1;
            }
            else
            {
                Y = -1;
            }
        }
    }
}
