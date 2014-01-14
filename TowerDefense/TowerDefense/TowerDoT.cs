using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TowerDefense
{
    class TowerDoT : Tower
    {
        // test

        //Fields
        private float dmgOverTime;
        private float duration;

        //Properties
        public float DmgOverTime
        {
            get { return dmgOverTime; }
            set { dmgOverTime = value; }
        }

        public float Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        //Constructor
        public TowerDoT(float dmgOverTime, float duration, float speed, int cost, int ranged,string imagePath, PointF position) : base(speed, cost, ranged, imagePath, position)
        {
            this.dmgOverTime = dmgOverTime;
            this.duration = duration;
            this.Bullet = new Projectile(10, 5, @"Towers/w2.png", position, this);
        }

        public void DoT()
        {
            
        }

        public override void Update(float FPS)
        {
            base.Update(FPS);
        }
    }
}
