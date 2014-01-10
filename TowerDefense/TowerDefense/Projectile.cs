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
        public int damage;
        public float bulletSpeed;
        private Tower tw;
        private Enemy target;
        private int targetID;
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
        public int TargetID
        {
            get { return targetID; }
            set { targetID = value; }
        }
        public Tower Tw
        {
            get { return tw; }
            set { tw = value; }
        }

        /// <summary>
        /// Projectile Constructor
        /// </summary>
        /// <param name="damage"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        /// <param name="isClickable"></param>
        public Projectile( int damage, float bulletSpeed, string imagePath, PointF position, bool isClickable, Tower fromTower) : base(imagePath, position, isClickable)
        {
            this.damage = damage;
            this.bulletSpeed = bulletSpeed;
            this.tw = fromTower;
        }
        /// <summary>
        /// lucas
        /// Moves the bullet along a vector to target and give target damage
        /// Remove bullet when bullet position is equal to target position
        /// </summary>
        public void MoveBullet()
        {
            if (Form1.gw.currentWave[targetID].Enabled)
            {
                target = Form1.gw.currentWave[targetID];
                Vector2D direction = new Vector2D(target.Position.X - position.X, target.Position.Y - position.Y);
                direction.Normalize();
                if (direction.X > 0 && direction.Y > 0)
                    position = new PointF(Math.Abs(direction.X * bulletSpeed) + position.X, Math.Abs(direction.Y * bulletSpeed) + position.Y);
                if (direction.X > 0 && direction.Y < 0)
                    position = new PointF(Math.Abs(direction.X * bulletSpeed) + position.X, position.Y - Math.Abs(direction.Y * bulletSpeed));
                if (direction.X < 0 && direction.Y > 0)
                    position = new PointF(position.X - Math.Abs(direction.X * bulletSpeed), Math.Abs(direction.Y * bulletSpeed) + position.Y);
                if (direction.X < 0 && direction.Y < 0)
                    position = new PointF(position.X - Math.Abs(direction.X * bulletSpeed), position.Y - Math.Abs(direction.Y * bulletSpeed));

                if (target.Position.X >= position.X && position.X <= target.Position.X + 90 && target.Position.Y <= position.Y && position.Y <= target.Position.Y + 90)
                {
                    if (damage > 0)
                    {
                        if (target is EnemyEvade)
                            if (CanHitEvadingEnemy((EnemyEvade) target))
                            {
                                Form1.gw.currentWave[targetID].HP = Form1.gw.currentWave[targetID].HP + Form1.gw.currentWave[targetID].Armor - damage;
                                Form1.gw.currentWave[targetID].OnImpact(dc);
                            }
                            else
                            {
                                Form1.gw.currentWave[targetID].HP = Form1.gw.currentWave[targetID].HP + Form1.gw.currentWave[targetID].Armor - damage;
                                Form1.gw.currentWave[targetID].OnImpact(dc);
                            }
                    }

                    if (Form1.gw.currentWave[targetID].HP <= 0)
                    {
                        Form1.gw.currentWave[targetID].Enabled = false;
                        Form1.gw.gold += Form1.gw.currentWave[targetID].Worth;
                    }
                    Form1.gw.bullets.Remove(this);
                    position = tw.Position;

                }
            }
            else
                Form1.gw.bullets.Remove(this);
        }
        public override void Update(float FPS)
        {
            MoveBullet();
            base.Update(FPS);
        }

        public bool CanHitEvadingEnemy(EnemyEvade evader)
        {
            if (evader.IsEvade)
                return false;
            else
            return true;
        }
    }
}
