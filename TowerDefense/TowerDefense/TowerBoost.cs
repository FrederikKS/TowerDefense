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
        private float boostDamage;
        private float boostSpeed;

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
        public TowerBoost(float boostDamage, float boostSpeed, float speed, int damage, int cost, int range, string imagePath, PointF position, bool isClickable) : base(speed, damage, cost, range, imagePath, position, isClickable)
        {
            this.boostDamage = boostDamage;
            this.boostSpeed = boostSpeed;
        }
    }
}
