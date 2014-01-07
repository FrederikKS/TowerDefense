﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public class Projectile : GameObject
    {
        


        //Fields
        private int damage;
        public float bulletSpeed;
        private Tower tw;
        private Enemy target;
        Graphics dc;


 

        //Properties
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
        }
        public Enemy Target
        {
            get { return target; }
            set { target = value; }
        }

        /// <summary>
        /// Projectile Constructor
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        /// <param name="isClickable"></param>
        public Projectile( int damage, float bulletSpeed, string imagePath, PointF position, bool isClickable, Tower fromTower ) : base(imagePath, position, isClickable)
        {
            this.damage = damage;
            this.bulletSpeed = bulletSpeed;
            this.tw = fromTower;
        }
        /// <summary>
        /// lucas
        /// Moves the bullet along a vector to target and give target damage
        /// Remove bullet when bullet position are equal to target position
        /// </summary>
        public void MoveBullet()
        {
            Vector2D direction = new Vector2D(target.Position.X - Position.X, target.Position.Y - Position.Y);
            direction.Normalize(bulletSpeed);
            Position = new PointF(direction.X + Position.X, direction.Y + Position.Y);
            if (target.Position == position)
            {
                target.HP = target.HP + target.Armor - damage;
                target.OnImpact(dc);
                if (target.HP <= 0)
                {
                   Form1.gw.currentWave.Remove(target);
                }
                Form1.gw.bullets.Remove(this);
                
            }
        }
        public override void Update(float FPS)
        {
            MoveBullet();
            base.Update(FPS);
        }
    }
}
