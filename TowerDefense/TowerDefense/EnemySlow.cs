using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace TowerDefense
{
    class EnemySlow : Enemy
    {
        // test
        //Field
        private float eSlow;
        private int eRange;
        //Properties

        //Constructer
        public EnemySlow(string name, float eSlow, int eRange, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition): base(name,hp,speed,armor,worth,effect,imagePath,position,endPosition)
        {
            this.eSlow = eSlow;
            this.eRange = eRange;
        }

        /// <summary>
        /// Mikkel
        /// Slows all nearby towers
        /// </summary>
        public void EnemySlowing()
        {
            for (int i = 0; i < Form1.gw.towers.Count; i++)
            {
                if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.towers[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.towers[i].Position.Y), 2)) < eRange)
                {
                    if (Form1.gw.towers[i].speed != Form1.gw.towers[i].SavedSpeed && !Form1.gw.towers[i].Slowed)
                    {
                        Form1.gw.towers[i].speed -= Form1.gw.towers[i].speed * eSlow;
                        Form1.gw.towers[i].Slowed = true;
                    }
                }
                if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.towers[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.towers[i].Position.Y), 2)) > eRange && Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.towers[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.towers[i].Position.Y), 2)) < eRange + 2)
                {
                    if (Form1.gw.towers[i].Slowed && Form1.gw.towers[i].speed != Form1.gw.towers[i].SavedSpeed)
                    {
                        Form1.gw.towers[i].speed = Form1.gw.towers[i].SavedSpeed;
                        Form1.gw.towers[i].Slowed = false;
                    }
                }
            }
        }

        public override void Update(float fps)
        {
            EnemySlowing();
            base.Update(fps);           
        }

    }
}
