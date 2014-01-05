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
        public EnemySlow(string name, float eSlow, int eRange, float hp, int speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition, bool isClickable): base(name,hp,speed,armor,worth,effect,imagePath,position,endPosition,isClickable)
        {
            this.eSlow = eSlow;
            this.eRange = eRange;
        }

    }
}
