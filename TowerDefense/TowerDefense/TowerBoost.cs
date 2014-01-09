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
        public TowerBoost(int boostDamage, float boostSpeed, float speed, int cost, int ranged, string imagePath, PointF position, bool isClickable) 
            : base(speed, cost, ranged, imagePath, position, isClickable)
        {
            this.boostDamage = boostDamage;
            this.boostSpeed = boostSpeed;
            this.Bullet = new Projectile(10, 5, @"Towers/w2.png", position, false, this);
        }

        public void Boost()
        {
            // Runs through the list of placed towers and if they are within the range of the boost tower, they are boosted,
            // if tower is sold, then the boost is removed.
            for (int i = 0; i < Form1.gw.towers.Count; i++)
            {
                if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.towers[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.towers[i].Position.Y), 2)) < ranged)
                {
                    if (Form1.gw.towers[i].speed != 8)
                    {
                        Form1.gw.towers[i].speed += boostSpeed;
                    }
                }
                else if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.towers[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.towers[i].Position.Y), 2)) > ranged)
                {
                    if (Form1.gw.towers[i].speed >= 10)
                    {
                        Form1.gw.towers[i].speed -= boostSpeed;
                    }
                }
            }
            // Runs through the list of projectiles and if they are within the range of the boost tower, they are boosted.

            for (int i = 0; i < Form1.gw.bullets.Count; i++)
            {
                if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.bullets[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.bullets[i].Position.Y), 2)) < ranged)
                {
                    if (Form1.gw.bullets[i].damage != 4)
                    {
                        Form1.gw.bullets[i].damage += boostDamage;
                    }
                }
                else if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.bullets[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.bullets[i].Position.Y), 2)) > ranged)
                {
                    if (Form1.gw.bullets[i].damage >= 6)
                    {
                        Form1.gw.bullets[i].damage -= boostDamage;
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
