using System;
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
        
 

        //Properties
        public int Damage
        {
            get { return damage; }
            set { damage = value; }
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
            Vector2D direction = new Vector2D(tw.target.Position.X - Position.X, tw.target.Position.Y - Position.Y);
            direction.Normalize(bulletSpeed);
            Position = new PointF(direction.X + Position.X, direction.Y + Position.Y);
            if (tw.target.Position == position)
            {
                tw.target.HP = tw.target.HP + tw.target.Armor - damage;
                if (tw.target.HP <= 0)
                {
                   Form1.gw.currentWave.Remove(tw.target);
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
