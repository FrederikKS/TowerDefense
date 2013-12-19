using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TowerDefense
{
    class ParentNode : Node
    {
        //Fields
        private int locationX;
        private int locationY;
        private int nodeLocationX;
        private int nodeLocationY;
        private int f;
        private int g;
        private int h;
        private ParentNode parentNode;
        
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

        //Constructor
        public ParentNode(int locationX, int locationY, int endX, int endY, int parentNodeLocationX, int parentNodeLocationY) : base(locationX, locationY, endX,endY,parentNodeLocationX,parentNodeLocationY)
        {

        }
    }
}
