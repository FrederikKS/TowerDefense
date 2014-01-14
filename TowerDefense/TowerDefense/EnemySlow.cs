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
        public float ESlow
        {
            get { return eSlow; }
            set { eSlow = value; }
        }
        public int ERange
        {
            get { return eRange; }
            set { eRange = value; }
        }
        //Constructer
        public EnemySlow(string name, float eSlow, int eRange, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition): base(name,hp,speed,armor,worth,effect,imagePath,position,endPosition)
        {
            this.eSlow = eSlow;
            this.eRange = eRange;
        }

        public void EnemySlowing()
        {
            for (int i = 0; i < Form1.gw.towers.Count; i++)
            {
                if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.towers[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.towers[i].Position.Y), 2)) < eRange)
                {
                    if (Form1.gw.towers[i].speed == 5)
                    {
                        Form1.gw.towers[i].speed -= eSlow;
                    }
                }
                else if (Math.Sqrt(Math.Pow(Math.Abs(position.X - Form1.gw.towers[i].Position.X), 2) + Math.Pow(Math.Abs(position.Y - Form1.gw.towers[i].Position.Y), 2)) > eRange)
                {
                    if (Form1.gw.towers[i].speed == eSlow + 1)
                    {
                        Form1.gw.towers[i].speed = 5;
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
