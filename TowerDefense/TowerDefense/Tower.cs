using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;

namespace TowerDefense
{
    public abstract class Tower : GameObject
    {
        
        // Field
        private Stopwatch stopWatch = new Stopwatch();
        public float speed;
        private int cost;
        protected float ranged;
        private Projectile bullet;
        private bool boosted = false;
        private int savedDamage;
 
        // Property
        public float Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public Projectile Bullet
        {
            get { return bullet; }
            set { bullet = value; }
        }
        public int Cost
        {
            get { return cost; }
        }
        public bool Boosted
        {
            get { return boosted; }
            set { boosted = value; }
        }
        public int SavedDamage
        {
            get { return savedDamage; }
            set { savedDamage = value; }
        }
        /// <summary>
        /// The Constructor For The Tower Class
        /// </summary>
        /// <param name="speed"></param>
        /// <param name="damage"></param>
        /// <param name="cost"></param>
        /// <param name="ranged"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        /// <param name="isClickAble"></param>
        public Tower(float speed, int cost, float ranged, string imagePath, PointF position) : base(imagePath, position)
        {

            this.speed = speed;
            this.cost = cost;
            this.ranged = ranged;

            stopWatch.Start();
            

        }

        /// <summary>
        /// lucas
        /// checks if enemy are in range and checks if cooldown is down
        /// mark the enemy target
        /// </summary>
        public void Atk()
        {
            for (int i = 0; i < Form1.gw.currentWave.Count; i++)
            {
                if (Form1.gw.currentWave[i].Enabled)
                if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.currentWave[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.currentWave[i].Position.Y), 2)) < ranged)
                {
                    if (stopWatch.ElapsedMilliseconds > speed)
                    {
                        if (bullet != null)
                        {
                            bullet.Target = Form1.gw.currentWave[i];
                            bullet.TargetID = i;

                            if (Form1.gw.currentWave[i].Enabled)
                            {
                                Form1.gw.bullets.Add(bullet);
                                stopWatch.Restart();
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// The Update function for the towers
        /// </summary>
        /// <param name="FPS"></param>
        public override void Update(float FPS) 
        {
            Atk();
            base.Update(FPS);
        }

    }
}
