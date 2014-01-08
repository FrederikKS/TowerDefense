using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace TowerDefense
{
    class EnemyEvade : Enemy
    {
        // test
        //Field
        private bool isEvade;
        private Timer evadeTime = new Timer(20000);
         
        //Properties
        public bool IsEvade
        {
            set { isEvade = value; }
        }
        //Constructor
        public EnemyEvade(string name, bool isEavde, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition, bool isClickable)
            : base(name, hp, speed, armor, worth, effect, imagePath, position, endPosition, isClickable)
        {
            this.isEvade = isEavde;
        }

        public void Timer ()
        {
            evadeTime.Enabled = true;
            evadeTime.Interval = 2000;
            evadeTime.Elapsed += new ElapsedEventHandler(Evade);
        }
        public void Evade(object source, ElapsedEventArgs e) 
        {
            while (1000 > DateTime.Now.Millisecond)
            {

                for (int i = 0; i < Form1.gw.bullets.Count; i++)
                {
                    //if (Form1.gw.bullets[i].Position == position)
                    //{
                    //    Form1.gw.bullets[i].Damage = 0;
                    //}
                }
            }
        }
        
        public override void Update(float FPS)
        {
            Timer();
            
            base.Update(FPS);
        }
    }
}
