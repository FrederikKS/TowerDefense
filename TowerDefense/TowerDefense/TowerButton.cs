using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowerDefense
{   
    public class TowerButton
    {   
        private Rectangle collisionRect;

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public Rectangle CollisionRect
        {
            get { return collisionRect; }
            set { collisionRect = value; }
        }

        public TowerButton(Size mySize, Point myPosition, string name)
        {
            collisionRect = new Rectangle(myPosition, mySize);
            this.name = name;
        }

        public void DrawMe(Graphics dc)
        {
            //Draw example button
            dc.FillRectangle(Brushes.Green, collisionRect);
        }

    }
}
