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
        public EnemyNormal(string name, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition): base(name,hp,speed,armor,worth,effect,imagePath,position,endPosition)
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
