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

        public TowerSlow(float slow, int range, float speed, int cost, float ranged, Projectile bullet, string imagePath, PointF position, bool isClickAble)
            : base(speed, cost, range, bullet, imagePath, position, isClickAble)
        {

        }

        public void Slowing()
        {
            for (int i = 0; i < Form1.gw.currentWave.Count; i++)
            {
                if (Math.Sqrt(position.X * Form1.gw.currentWave[i].Position.X + position.Y * Form1.gw.currentWave[i].Position.Y) > ranged && Form1.gw.currentWave[i].Speed != Form1.gw.currentWave[i].Speed - slow)
                {
                    Form1.gw.currentWave[i].Speed -= slow;
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
