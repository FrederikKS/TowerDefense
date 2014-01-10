using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Timers;

namespace TowerDefense
{
    public class EnemyEvade : Enemy
    {
        // test
        //Field
        private bool isEvade;
        private Timer evadeTime;

        //Properties
        public bool IsEvade
        {
            set { isEvade = value; }
            get { return isEvade; }
        }
        //Constructor
        public EnemyEvade(string name, bool isEvade, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition, bool isClickable)
            : base(name, hp, speed, armor, worth, effect, imagePath, position, endPosition, isClickable)
        {
            this.isEvade = isEvade;
        }

        public void State()
        {
            if (evadeTime == null)
                evadeTime = new Timer();
            evadeTime.Enabled = true;
            evadeTime.Interval = 5000;
            evadeTime.Elapsed += new ElapsedEventHandler(Evade);
        }
        public void Evade(object source, ElapsedEventArgs e)
        {
            if (isEvade == true)
            {
                isEvade = false;
            }
            else
                isEvade = true;
        }

        public override void Update(float FPS)
        {
            State();
            base.Update(FPS);
            if (Enabled == false)
                evadeTime.Enabled = false;
        }
    }
}
