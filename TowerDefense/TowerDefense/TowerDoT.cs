﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;

namespace TowerDefense
{
    class TowerDoT : TowerType
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
        public TowerDoT(float dmgOverTime, float duration, float speed, int cost, int ranged, Projectile bullet,string imagePath, PointF position, bool isClickable) : base(speed, cost, ranged, bullet, imagePath, position, isClickable)
        {
            this.dmgOverTime = dmgOverTime;
            this.duration = duration;
        }
    }
}
