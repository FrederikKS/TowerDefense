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
        // test

        // Field
        private Stopwatch stopWatch = new Stopwatch();
        public float speed;
        private int cost;
        protected float ranged;

        private Projectile bullet;


        public Enemy target;
 
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
        public Tower(float speed, int cost, float ranged, string imagePath, PointF position, bool isClickable) : base(imagePath, position, isClickable)
        {
            this.speed = speed;
            this.cost = cost;
            this.ranged = ranged;
            
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
                if (Math.Sqrt(position.X * Form1.gw.currentWave[i].Position.X + position.Y * Form1.gw.currentWave[i].Position.Y) < ranged)
                {
                    stopWatch.Start();
                    if (stopWatch.Elapsed.Milliseconds > speed)
                    {

                        if (target == null)
                        {
                            target = Form1.gw.currentWave[i];
                        }
                        Form1.gw.bullets.Add(bullet);
                        stopWatch.Restart();
                    }
                }
                else
                {
                    target = null;
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
