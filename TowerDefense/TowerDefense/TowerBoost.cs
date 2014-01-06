using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TowerDefense
{
    class TowerBoost : TowerType
    {
        //Fields
        private int boostDamage;
        private float boostSpeed;
        private GameWorld gw;
        private Projectile projectile;
        
        //Properties
        public int BoostDamage
        {
            get { return boostDamage; }
            set { boostDamage = value; }
        }

        public float BoostSpeed
        {
            get { return boostSpeed; }
            set { boostSpeed = value; }
        }

        //Constructor
        public TowerBoost(int boostDamage, float boostSpeed, float speed, int cost, int ranged, Projectile bullet, string imagePath, PointF position, bool isClickable) 
            : base(speed, cost, ranged, bullet, imagePath, position, isClickable)
        {
            this.boostDamage = boostDamage;
            this.boostSpeed = boostSpeed;
        }

        public void Boost()
        {
            for (int i = 0; i < gw.currentWave.Count; i++)
            {
                if (Math.Sqrt(position.X * gw.currentWave[i].Position.X + position.Y * gw.currentWave[i].Position.Y) > ranged)
                {
                    foreach (Tower tw in gw.towers)
                    {
                        tw.speed += boostSpeed;
                        projectile.Damage += boostDamage;
                        
                    }
                }
            }
        }

        public override void Update(float FPS)
        {
            Boost();
            base.Update(FPS);
        }
    }
}
