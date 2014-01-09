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

        public static Vector2D operator +(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X + v2.X, v1.Y + v2.Y);
        }

        public static Vector2D operator -(Vector2D v1, Vector2D v2)
        {
            return new Vector2D(v1.X - v2.X, v1.Y - v2.Y);
        }

        public static Vector2D operator *(Vector2D v1, float m)
        {
            return new Vector2D(v1.X * m, v1.Y * m);
        }
        public static float operator *(Vector2D v1, Vector2D v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        public static Vector2D operator /(Vector2D v1, float m)
        {
            return new Vector2D(v1.X / m, v1.Y / m);
        }

        public static float Distance(Vector2D v1, Vector2D v2)
        {
            return (float)Math.Sqrt(Math.Pow(v1.X - v2.X, 2) + Math.Pow(v1.Y - v2.Y, 2));
        }

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

        public float Length()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }
    }
}
