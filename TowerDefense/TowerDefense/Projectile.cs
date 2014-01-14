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
        private float correctionX = 0;
        private float correctionY = 0;

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
        public Projectile(int damage, float bulletSpeed, string imagePath, PointF position, Tower fromTower) : base(imagePath, position)
        {
            this.damage = damage;
            this.bulletSpeed = bulletSpeed;
            this.tw = fromTower;
        }
        /// <summary>
        /// Lucas & Frederik
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
                {
                    if (bulletSpeed > Math.Abs(target.Position.X - position.X))
                        correctionX = Math.Abs(target.Position.X - position.X);
                    else
                        correctionX = bulletSpeed;
                    if (bulletSpeed > Math.Abs(target.Position.Y - position.Y))
                        correctionY = Math.Abs(target.Position.Y - position.Y);
                    else
                        correctionY = bulletSpeed;

                    position = new PointF(Math.Abs(direction.X * correctionX) + position.X, Math.Abs(direction.Y * correctionY) + position.Y);
                }
                if (direction.X > 0 && direction.Y < 0)
                {
                    if (bulletSpeed > Math.Abs(target.Position.X - position.X))
                        correctionX = Math.Abs(target.Position.X - position.X);
                    else
                        correctionX = bulletSpeed;
                    if (bulletSpeed > Math.Abs(target.Position.Y - position.Y))
                        correctionY = Math.Abs(target.Position.Y - position.Y);
                    else
                        correctionY = bulletSpeed;

                    position = new PointF(Math.Abs(direction.X * correctionX) + position.X, position.Y - Math.Abs(direction.Y * correctionY));
                }

                if (direction.X < 0 && direction.Y > 0)
                {
                    if (bulletSpeed > Math.Abs(target.Position.X - position.X))
                        correctionX = Math.Abs(target.Position.X - position.X);
                    else
                        correctionX = bulletSpeed;
                    if (bulletSpeed > Math.Abs(target.Position.Y - position.Y))
                        correctionY = Math.Abs(target.Position.Y - position.Y);
                    else
                        correctionY = bulletSpeed;

                    position = new PointF(position.X - Math.Abs(direction.X * correctionX), Math.Abs(direction.Y * correctionY) + position.Y);
                }

                if (direction.X < 0 && direction.Y < 0)
                {
                    if (bulletSpeed > Math.Abs(target.Position.X - position.X))
                        correctionX = Math.Abs(target.Position.X - position.X);
                    else
                        correctionX = bulletSpeed;
                    if (bulletSpeed > Math.Abs(target.Position.Y - position.Y))
                        correctionY = Math.Abs(target.Position.Y - position.Y);
                    else
                        correctionY = bulletSpeed;

                    position = new PointF(position.X - Math.Abs(direction.X * correctionX), position.Y - Math.Abs(direction.Y * correctionY));
                }

                //Checks if projectile's position is close enough to 'hit' enemy
                if (position.X < target.Position.X + 10 && position.X > target.Position.X - 10 && position.Y < target.Position.Y + 10 && position.Y > target.Position.Y - 10)
                {
                    if (damage > 0)
                    {
                        if (Form1.gw.currentWave[targetID] is EnemyEvade)
                        {
                            if (CanHitEvadingEnemy((EnemyEvade)Form1.gw.currentWave[targetID]))
                            {
                                Form1.gw.currentWave[targetID].HP = Form1.gw.currentWave[targetID].HP + Form1.gw.currentWave[targetID].Armor - damage;
                                Form1.gw.currentWave[targetID].OnImpact();
                            }
                            else
                                Form1.gw.bullets.Remove(this);
                        }
                        else
                        {
                            Form1.gw.currentWave[targetID].HP = Form1.gw.currentWave[targetID].HP + Form1.gw.currentWave[targetID].Armor - damage;
                            Form1.gw.currentWave[targetID].OnImpact();
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
                correctionX = 0;
                correctionY = 0;
            }
            else
                Form1.gw.bullets.Remove(this);
        }
        public override void Update(float FPS)
        {
            MoveBullet();
            base.Update(FPS);
        }

        /// <summary>
        /// Checks if targeted enemy is evading
        /// </summary>
        /// <param name="evader"></param>
        /// <returns></returns>
        public bool CanHitEvadingEnemy(EnemyEvade evader)
        {
            if (evader.IsEvade)
                return false;
            else
                return true;
        }
    }
}
