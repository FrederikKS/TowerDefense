using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public class TowerSlow : TowerType
    {
        // test

        // Field
        private float slow;
        private int range;
        // Property
        public float Slow
        {
            get { return slow; }
        }
        public int Range
        {
            get { return range; }
        }

        public TowerSlow(float slow, int range, float speed, int damage, int cost, string imagePath, PointF position, bool isClickAble)
            : base(speed, damage, cost, range, imagePath, position, isClickAble)
        {

        }
    }
}
