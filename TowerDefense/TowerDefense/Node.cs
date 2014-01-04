using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense
{
    public class Node
    {
        //Fields
        private int locationX;
        private int locationY;
        private int nodeLocationX;
        private int nodeLocationY;
        private int f;
        private int g;
        private int h;
        private bool wasChecked = false;

        //Properties
        public int LocationX
        {
            get { return locationX; }
            set { locationX = value; }
        }

        public int LocationY
        {
            get { return locationY; }
            set { locationY = value; }
        }

        public int F
        {
            get { return f; }
            set { f = value; }
        }

        public int G
        {
            get { return g; }
            set { g = value; }
        }

        public int H
        {
            get { return h; }
            set { h = value; }
        }

        public bool WasChecked
        {
            get { return wasChecked; }
            set { wasChecked = value; }
        }

        //Constructor
        public Node(int locationX, int locationY, int endX, int endY, int parentNodeGValue)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.H = Math.Abs(locationX - endX) + Math.Abs(locationY - endY);
            this.G = parentNodeGValue + 1;
            this.F = H + G;

        }
    }
}
