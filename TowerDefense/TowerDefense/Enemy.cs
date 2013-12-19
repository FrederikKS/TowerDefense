using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
namespace TowerDefense
{
    public abstract class Enemy : GameObject
    {
        // test
        //fields
        #region
        private int hp;
        private int speed;
        private int armor;
        private int worth;
        private Effect effect;
        #endregion
        //propertis
        #region
        public int HP
        {
            get { return hp; }
            set { hp = value; }
        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        public int Armor
        {
            get { return armor; }
            set { armor = value; }
        }
        public int Worth
        {
            get { return worth; }
        }
        #endregion
        //constructor
        public Enemy( int hp, int speed, int armor, int worth, Effect effect, string imagePath, PointF position, bool isClickable): base(imagePath,position,isClickable)
        {
            this.hp = hp;
            this.speed = speed;
            this.armor = armor;
            this.worth = worth;
            this.effect = effect;
        }

    }
}
