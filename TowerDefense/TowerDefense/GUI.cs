﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace TowerDefense
{
    public class GUI
    {
        GameWorld gw;
        // Field
        int tileSizeX = 96;
        int tileSizeY = 96;
        public Rectangle ellipse;
        // Property
        // Classes
        TowerButton b;
        /// <summary>
        /// Draws The GUI (Build and Sell Menu)
        /// </summary>
        /// <param name="g"></param>
        public void DrawGUI(Graphics g)
        {
            if (gw == null)
            {
                gw = new GameWorld(g, Form1.ActiveForm.DisplayRectangle, 14, 8, Form1.difc);
            }
            // Tower Tile
            if (Form1.drawBuildGUI == 2)
            {
                // Tower 1
                Point point1 = new Point(Form1.guiPos.X, Form1.guiPos.Y);
                Point point2 = new Point(Form1.guiPos.X, Form1.guiPos.Y + 150);
                // Tower 2
                Point point3 = new Point(Form1.guiPos.X, Form1.guiPos.Y);
                Point point4 = new Point(Form1.guiPos.X - 120, Form1.guiPos.Y - 90);
                // Tower 3
                Point point5 = new Point(Form1.guiPos.X, Form1.guiPos.Y);
                Point point6 = new Point(Form1.guiPos.X + 120, Form1.guiPos.Y - 90);
                // Pen
                Pen p = new Pen(Color.Red);
                Pen pp = new Pen(Color.Blue);
                Pen ppp = new Pen(Color.Green);
                // The Large Ellipse
                g.DrawEllipse(p, Form1.guiPos.X - 150, Form1.guiPos.Y - 150, 300, 300);
                // Drawing the lines
                g.DrawLine(p, point1, point2);
                g.DrawLine(pp, point3, point4);
                g.DrawLine(ppp, point5, point6);
                // The Small Ellipse
                g.DrawEllipse(p, Form1.guiPos.X - 50, Form1.guiPos.Y - 50, 100, 100);
                // Tower Buttons
                //b = new TowerButton(new Size(100, 100), new Point(150, 150), "Test", "hej", 1);

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
                // Tower 1
                Point point1 = new Point(Form1.guiPos.X, Form1.guiPos.Y);
                Point point2 = new Point(Form1.guiPos.X, Form1.guiPos.Y + 150);
                // Tower 2
                Point point3 = new Point(Form1.guiPos.X, Form1.guiPos.Y);
                Point point4 = new Point(Form1.guiPos.X - 120, Form1.guiPos.Y - 90);
                // Tower 3
                Point point5 = new Point(Form1.guiPos.X, Form1.guiPos.Y);
                Point point6 = new Point(Form1.guiPos.X + 120, Form1.guiPos.Y - 90);
                // Pen
                Pen p = new Pen(Color.Brown);
                Pen pp = new Pen(Color.Brown);
                Pen ppp = new Pen(Color.Brown);
                // The Large Ellipse
                g.DrawEllipse(p, Form1.guiPos.X - 150, Form1.guiPos.Y - 150, 300, 300);
                // Drawing the lines
                g.DrawLine(p, point1, point2);
                g.DrawLine(pp, point3, point4);
                g.DrawLine(ppp, point5, point6);
                // The Small Ellipse
                g.DrawEllipse(p, Form1.guiPos.X - 50, Form1.guiPos.Y - 50, 100, 100);
            }
        }
    }
}
