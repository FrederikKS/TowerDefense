using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public class TowerSlow : Tower
    {
        // Field
        private float slow;
        private int range;
        // Property
        public TowerSlow(float slow, int range, float speed, int cost, float ranged, string imagePath, PointF position)
            : base(speed, cost, range, imagePath, position)
        {
            this.slow = slow;
            this.range = range;
        }

        /// <summary>
        /// Mikkel
        /// Slows nearby enemies
        /// </summary>
        public void Slowing()
        {
            // Runs through every enemy in currentWave, if they are within range they are slowed

            for (int i = 0; i < Form1.gw.currentWave.Count; i++)
            {
                if (Math.Sqrt(Math.Pow(Math.Abs(this.Position.X - Form1.gw.currentWave[i].Position.X), 2) + Math.Pow(Math.Abs(this.Position.Y - Form1.gw.currentWave[i].Position.Y), 2)) < range)
                {
                    if (Form1.gw.currentWave[i].Speed == Form1.gw.currentWave[i].SpeedOriginal)
                    {
                        Form1.gw.currentWave[i].Speed = Form1.gw.currentWave[i].Speed * slow;
                    }
                }
                // If they exit range once again, their original speed is restored

                if (Math.Sqrt(Math.Pow(Math.Abs(this.Position.X - Form1.gw.currentWave[i].Position.X), 2) + Math.Pow(Math.Abs(this.Position.Y - Form1.gw.currentWave[i].Position.Y), 2)) > range && Form1.gw.currentWave[i].Speed != Form1.gw.currentWave[i].SpeedOriginal)
                {
                    Form1.gw.currentWave[i].Speed = Form1.gw.currentWave[i].SpeedOriginal;
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
