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
                int tmpX = Form1.gui.ellipse.X + Form1.gw.tileSizeX + (Form1.gw.tileSizeX / 2);
                int tmpY = Form1.gui.ellipse.Y + Form1.gw.tileSizeY + (Form1.gw.tileSizeY / 2);

                if (Form1.drawBuildGUI == 4)
                {
                    dc.DrawImage(Image.FromFile(@"Towers/L1.png"), new Point(tmpX + 50, tmpY));
                    dc.DrawImage(Image.FromFile(@"Towers/L2.png"), new Point(tmpX - 120, tmpY));
                    dc.DrawImage(Image.FromFile(@"Towers/L3.png"), new Point(tmpX - 30, tmpY - 120));
                    //dc.FillRectangle(Brushes.Brown, collisionRect);
                }
                if (Form1.drawBuildGUI == 3)
                {
                    dc.DrawImage(Image.FromFile(@"Towers/w1.png"), new Point(tmpX + 50, tmpY));
                    dc.DrawImage(Image.FromFile(@"Towers/w2.png"), new Point(tmpX - 120, tmpY));
                    dc.DrawImage(Image.FromFile(@"Towers/w3.png"), new Point(tmpX - 30, tmpY - 120));
                    //dc.FillRectangle(Brushes.Blue, collisionRect);
                }
                if (Form1.drawBuildGUI == 2)
                {
                    dc.DrawImage(Image.FromFile(@"Towers/st.png"), new Point(tmpX - 30, tmpY - 120));
                    //dc.FillRectangle(Brushes.Red, collisionRect);
                }
                // Draw The TowerButton

            }
        }
    }
}
