using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace TowerDefense
{
    class EnemyEvade : Enemy
    {
        // test
        //Field
        private bool isEvade;
        //Properties
        public bool IsEvade
        {
            set { isEvade = value; }
        }
        //Constructor
        public EnemyEvade(bool isEvade, int hp, int speed, int armor, int worth, Effect effect, string imagePath, PointF position, bool isClickable) : base(hp,speed,armor,worth, effect,imagePath,position,isClickable)
        {
            this.isEvade = isEvade;
        }
        //Functions
        /// <summary>
        /// Overrides Update function
        /// </summary>
        /// <param name="FPS"></param>
        public override void Update(float FPS)
        {

        }

    }
}
