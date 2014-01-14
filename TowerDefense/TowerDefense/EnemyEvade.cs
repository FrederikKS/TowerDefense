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
        public EnemyEvade(string name, bool isEvade, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition)
            : base(name, hp, speed, armor, worth, effect, imagePath, position, endPosition)
        {
            this.isEvade = isEvade;
            State();
        }

        /// <summary>
        /// Frederik
        /// Changes enemy's state from evading to not evading, depending on
        /// </summary>
        public void State()
        {
            if (evadeTime == null)
                evadeTime = new Timer();
            evadeTime.Enabled = true;
            evadeTime.Interval = 3000;
            evadeTime.Elapsed += new ElapsedEventHandler(Evade);
        }

        /// <summary>
        /// Frederik
        /// Reverts enemy's evade status after timer elapses
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
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
            
            base.Update(FPS);
            if (Enabled == false)
                evadeTime.Enabled = false;
            if (Enabled && evadeTime.Enabled == false)
                evadeTime.Enabled = true;
        }
    }
}
