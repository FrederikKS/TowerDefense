﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TowerDefense
{
    public class GUI
    {
        // Fields
        int x = 0;
        int y = 0;
        // Property
        /// <summary>
        /// Draws The GUI (Build and Sell Menu)
        /// </summary>
        /// <param name="g"></param>
        public void DrawGUI(Graphics g)
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
        }

        // Functions
    }
}