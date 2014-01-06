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
            if (Form1.guiIsClicked)
            {
                if (Form1.drawBuildGUI == 4)
                {
                    dc.FillRectangle(Brushes.Brown, collisionRect);
                }
                if (Form1.drawBuildGUI == 3)
                {
                    dc.FillRectangle(Brushes.Blue, collisionRect);
                }
                if (Form1.drawBuildGUI == 2)
                {
                    dc.FillRectangle(Brushes.Red, collisionRect);
                }
                // Draw The TowerButton

            }
        }
    }
}
