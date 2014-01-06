using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TowerDefense
{
    public class GUI
    {
        // Field
        int tileSizeX = 96;
        int tileSizeY = 96;
        public Rectangle ellipse;
        /// <summary>
        /// Draws The GUI (Build and Sell Menu)
        /// </summary>
        /// <param name="g"></param>
        public void DrawGUI(Graphics g)
        {
            // Tower Tile
            if (Form1.drawBuildGUI == 2)
            {
                int x = (((int)Math.Floor((decimal)Form1.guiPos.X / tileSizeX)) * tileSizeX) - 1 * tileSizeX;
                int y = (((int)Math.Floor((decimal)Form1.guiPos.Y / tileSizeY)) * tileSizeY) - 1 * tileSizeY;

                ellipse = new Rectangle(x, y, tileSizeX * 3, tileSizeY * 3);
                // Place the GUI's Position to center of tile
                #region Tower Pen Collor
                // Pen
                Pen p = new Pen(Color.Red);
                Pen pp = new Pen(Color.Red);
                Pen ppp = new Pen(Color.Red);
                #endregion


                // The Large Ellipse
                g.DrawEllipse(p, ellipse);
                #region Tower Walls
                // TowerWall 1
                Point center = new Point((ellipse.X + tileSizeX / 2) + tileSizeX, (ellipse.Y + tileSizeY / 2) + tileSizeY);
                Point point2 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX, ((ellipse.Y + tileSizeY / 2) + tileSizeY) + (tileSizeY * 3 / 2));
                // TowerWall 2
                Point point4 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX - 120, (ellipse.Y + tileSizeY / 2) + tileSizeY - 90);
                // TowerWall 3
                Point point6 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX + 120, (ellipse.Y + tileSizeY / 2) + tileSizeY - 90);
                #endregion
                // Drawing the lines
                g.DrawLine(p, center, point2);
                g.DrawLine(pp, center, point4);
                g.DrawLine(ppp, center, point6);

            }
            // Water Tile
            if (Form1.drawBuildGUI == 3)
            {
                int x = (((int)Math.Floor((decimal)Form1.guiPos.X / tileSizeX)) * tileSizeX) - 1 * tileSizeX;
                int y = (((int)Math.Floor((decimal)Form1.guiPos.Y / tileSizeY)) * tileSizeY) - 1 * tileSizeY;

                ellipse = new Rectangle(x, y, tileSizeX * 3, tileSizeY * 3);
                // Place the GUI's Position to center of tile
                #region Tower Pen Collor
                // Pen
                Pen p = new Pen(Color.White);
                Pen pp = new Pen(Color.White);
                Pen ppp = new Pen(Color.White);
                #endregion


                // The Large Ellipse
                g.DrawEllipse(p, ellipse);
                #region Tower Walls
                // TowerWall 1
                Point center = new Point((ellipse.X + tileSizeX / 2) + tileSizeX, (ellipse.Y + tileSizeY / 2) + tileSizeY);
                Point point2 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX, ((ellipse.Y + tileSizeY / 2) + tileSizeY) + (tileSizeY * 3 / 2));
                // TowerWall 2
                Point point4 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX - 120, (ellipse.Y + tileSizeY / 2) + tileSizeY - 90);
                // TowerWall 3
                Point point6 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX + 120, (ellipse.Y + tileSizeY / 2) + tileSizeY - 90);
                #endregion
                // Drawing the lines
                g.DrawLine(p, center, point2);
                g.DrawLine(pp, center, point4);
                g.DrawLine(ppp, center, point6);
            }
            // Island Tile
            if (Form1.drawBuildGUI == 4)
            {
                int x = (((int)Math.Floor((decimal)Form1.guiPos.X / tileSizeX)) * tileSizeX) - 1 * tileSizeX;
                int y = (((int)Math.Floor((decimal)Form1.guiPos.Y / tileSizeY)) * tileSizeY) - 1 * tileSizeY;

                ellipse = new Rectangle(x, y, tileSizeX * 3, tileSizeY * 3);
                // Place the GUI's Position to center of tile
                #region Tower Pen Collor
                // Pen
                Pen p = new Pen(Color.Brown);
                Pen pp = new Pen(Color.Brown);
                Pen ppp = new Pen(Color.Brown);
                #endregion


                // The Large Ellipse
                g.DrawEllipse(p, ellipse);
                #region Tower Walls
                // TowerWall 1
                Point center = new Point((ellipse.X + tileSizeX / 2) + tileSizeX, (ellipse.Y + tileSizeY / 2) + tileSizeY);
                Point point2 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX, ((ellipse.Y + tileSizeY / 2) + tileSizeY) + (tileSizeY * 3 / 2));
                // TowerWall 2
                Point point4 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX - 120, (ellipse.Y + tileSizeY / 2) + tileSizeY - 90);
                // TowerWall 3
                Point point6 = new Point((ellipse.X + tileSizeX / 2) + tileSizeX + 120, (ellipse.Y + tileSizeY / 2) + tileSizeY - 90);
                #endregion
                // Drawing the lines
                g.DrawLine(p, center, point2);
                g.DrawLine(pp, center, point4);
                g.DrawLine(ppp, center, point6);
            }
        }
    }
}
