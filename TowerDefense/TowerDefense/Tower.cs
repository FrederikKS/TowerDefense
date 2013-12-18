﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace TowerDefense
{
    public abstract class Tower : GameObject
    {
        // Field
        private Stopwatch stopWatch;
        private int speed;
        private int damage;
        private int cost;
        private float ranged;
        // Property
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Damage
        {
            get { return damage; }
        }
        /// <summary>
        /// The COnstructor for the Tower Class
        /// </summary>
        public Tower(int speed, int damage, int cost, float ranged, string imagePath, PointF position, bool isClickAble) : base(string imagePath, PointF position, bool isClickAble)
        {
            this.speed = speed;
            this.damage = damage;
            this.cost = cost;
            this.ranged = ranged;
        }
    }
}
