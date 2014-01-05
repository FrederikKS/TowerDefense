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
        private float hp;
        private int speed;
        private int armor;
        private int worth;
        private Effect effect;
        private string name;
        private PointF endPosition;
        private int reachedPointCounter = 0;
        private int reachedEndCounter = 0;


        #endregion
        //propertis
        #region
        public string Name 
        {
            get { return name; }
            set { name = value; }
        }

        public float HP
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
        public PointF EndPosition
        {
            get { return endPosition; }
            set { endPosition = value; }
        }
        public int ReachedPointCounter
        {
            get { return reachedPointCounter; }
            set { reachedPointCounter = value; }
        }
        public int ReachedEndCounter
        {
            get { return reachedEndCounter; }
            set { reachedEndCounter = value; }
        }
        #endregion
        //constructor
        public Enemy(string name, float hp, int speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition, bool isClickable): base(imagePath,position,isClickable)
        {
            this.name = name;
            this.hp = hp;
            this.speed = speed;
            this.armor = armor;
            this.worth = worth;
            this.effect = effect;
            this.endPosition = endPosition;
        }

        public void MovementAI()
        {
            if (position.X < endPosition.X)
            {
                position.X = position.X + speed;
            }

            if (position.X > endPosition.X)
            {
                position.X = position.X - speed;
            }


            if (position.Y < endPosition.Y)
            {
                position.Y = position.Y + speed;
            }

            if (position.Y > endPosition.Y)
            {
                position.Y = position.Y - speed;
            }

        }

        public virtual void Update(float fps)
        {
            MovementAI();
            base.Update(fps);
        }

    }
}
