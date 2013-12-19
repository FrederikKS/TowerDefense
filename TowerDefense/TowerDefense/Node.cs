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
        private Node parentNode;
        
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

        public int NodeLocationX
        {
            get { return nodeLocationX; }
            set { nodeLocationX = value; }
        }

        public int NodeLocationY
        {
            get { return nodeLocationY; }
            set { nodeLocationY = value; }
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

        public Node ParentNode
        {
            get { return parentNode; }
            set { parentNode = value; }
        }

        //Constructor
        public Node(int locationX, int locationY, int endX, int endY, int parentNodeLocationX, int parentNodeLocationY)
        {
            this.locationX = locationX;
            this.locationY = locationY;
            this.H = FixNeg(locationX - endX) + FixNeg(locationY - endY);
            this.G = FixNeg(locationX - parentNodeLocationX) + FixNeg(locationY - parentNodeLocationY);
            this.F = H + G;

        }

        //Correcting negative numbers
        public int FixNeg(int value1)
        {
            if (value1 < 0)
                value1 = value1 * -1;

            return value1;
        }
    }
}
