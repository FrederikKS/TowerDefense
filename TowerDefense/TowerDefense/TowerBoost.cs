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
        }

        public void Boost()
        {
            for (int i = 0; i < Form1.gw.towers.Count; i++)
            {
                if (Math.Sqrt(position.X * Form1.gw.towers[i].Position.X + position.Y * Form1.gw.towers[i].Position.Y) > ranged)
                {
                    
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
