using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public class TowerSlow : TowerType
    {
        // Field
        private float slow;
        private int range;
        // Property
        public float Slow
        {
            get { return slow; }
        }
        public int Range
        {
            get { return range; }
        }

        public TowerSlow(float slow, int range, float speed, int cost, float ranged, string imagePath, PointF position, bool isClickAble)
            : base(speed, cost, range, imagePath, position, isClickAble)
        {
            this.Bullet = new Projectile(100, 20, @"Graphic/CannonBall.png", position, false, this);
            this.slow = slow;
            this.range = range;
        }

        public void Slowing()
        {
            // Runs through every enemy in currentWave, if they are within range they are slowed

            for (int i = 0; i < Form1.gw.currentWave.Count; i++)
            {
                if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.currentWave[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.currentWave[i].Position.Y), 2)) < ranged)
                {
                    if (Form1.gw.currentWave[i].Speed > slow + 1)
                    {
                        Form1.gw.currentWave[i].Speed -= slow;
                    }
                }
                // If they exit range once again, their original speed is restored

                if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.currentWave[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.currentWave[i].Position.Y), 2)) > ranged)
                {
                    if (Form1.gw.currentWave[i].Speed == slow + 1)
                    {
                        Form1.gw.currentWave[i].Speed = 9;
                    }
                }
            }
        }

        public override void Update(float FPS)
        {
            Slowing();
            base.Update(FPS);
        }
    }
}
