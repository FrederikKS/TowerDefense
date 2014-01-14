using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TowerDefense
{
    class TowerBoost : Tower
    {
        //Fields
        private float boostDamage;
        private float boostSpeed;
        private int range;
        
        //Properties
        public float BoostDamage
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
        public TowerBoost(float boostDamage, float boostSpeed, float speed, int cost, int range, string imagePath, PointF position) 
            : base(speed, cost, range, imagePath, position)
        {
            this.boostDamage = boostDamage;
            this.boostSpeed = boostSpeed;
            this.range = range;
        }

        /// <summary>
        /// Mikkel
        /// Boosts nearby towers
        /// </summary>
        public void Boost()
        {
            // Runs through the list of placed towers and if they are within the range of the boost tower, they are boosted
            for (int i = 0; i < Form1.gw.towers.Count; i++)
            {
                if (Form1.gw.towers[i] is TowerNormal)
                {
                    if (Math.Sqrt(Math.Pow(Math.Abs(this.Position.X - Form1.gw.towers[i].Position.X), 2) + Math.Pow(Math.Abs(this.Position.Y - Form1.gw.towers[i].Position.Y), 2)) < range)
                    {
                        if (Form1.gw.towers[i].Boosted == false)
                        {
                            Form1.gw.towers[i].Bullet.Damage = Form1.gw.towers[i].Bullet.Damage + (int)(Form1.gw.towers[i].Bullet.Damage * boostDamage);
                            Form1.gw.towers[i].Boosted = true;
                        }
                    }
                    else
                    {
                        if (Form1.gw.towers[i].Boosted == true)
                        {
                            Form1.gw.towers[i].Bullet.Damage = Form1.gw.towers[i].SavedDamage;
                            Form1.gw.towers[i].Boosted = false;
                        }
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
