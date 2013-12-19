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
        // Fields
        private string name;
        private string imagePath;
        private int id;
                
        // Property
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
        public string ImagePath
        {
            get { return imagePath; }
            set { imagePath = value; }
        }
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        // Functions

        /// <summary>
        /// TowerButton COnstructor
        /// </summary>
        /// <param name="mySize"></param>
        /// <param name="myPosition"></param>
        /// <param name="name"></param>
        public TowerButton(Size mySize, Point myPosition, string name, string imagePath, int ID)
        {
            collisionRect = new Rectangle(myPosition, mySize);
            this.name = name;
        }
        /// <summary>
        /// Draws The rectangle
        /// </summary>
        /// <param name="dc"></param>
        public void DrawMe(Graphics dc)
        {
            
            //Draw example button
            dc.FillRectangle(Brushes.Green, collisionRect);
        }

    }
}
