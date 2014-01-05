using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace TowerDefense
{
    class EnemyNormal : Enemy
    {
        // test
        //Constructer
        public EnemyNormal(string name, float hp, int speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition, bool isClickable): base(name,hp,speed,armor,worth,effect,imagePath,position,endPosition,isClickable)
        {

        }

        public void normalAI()
        {
            
        }

        public override void Update(float fps)
        {
            normalAI();
            base.Update(fps);
        }
    }
}
