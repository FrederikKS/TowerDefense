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
        public EnemyNormal(int hp, int speed, int armor, int worth, Effect effect, string imagePath, PointF position, bool isClickable): base(hp,speed,armor,worth,effect,imagePath,position,isClickable)
        { }
    }
}
