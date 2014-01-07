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
        private float speed;
        private int armor;
        private int worth;
        private Effect effect;
        private string name;
        private PointF endPosition;
        private int reachedPointCounter = 0;
        private int reachedEndCounter = 0;
        private bool enabled = false;

        private Image explosion = Image.FromFile(@"Images/explosion.png");

        #endregion
        //properties
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
        public float Speed
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
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }
        #endregion
        //constructor
        public Enemy(string name, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition, bool isClickable): base(imagePath,position,isClickable)
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
            int correction = 0;
            
            if (position.X < endPosition.X)
            {
                if (Math.Abs(position.X - endPosition.X) < speed)
                {
                    correction = (int)Math.Abs(position.X - endPosition.X);
                    position.X = position.X + correction;
                }
                else
                    position.X = position.X + speed;

                correction = 0;
            }

            if (position.X > endPosition.X)
            {
                if (Math.Abs(position.X - endPosition.X) < speed)
                {
                    correction = (int)Math.Abs(position.X - endPosition.X);
                    position.X = position.X - correction;
                }
                else
                position.X = position.X - speed;

                correction = 0;
            }


            if (position.Y < endPosition.Y)
            {
                if (Math.Abs(position.Y - endPosition.Y) < speed)
                {
                    correction = (int)Math.Abs(position.Y - endPosition.Y);
                    position.Y = position.Y + correction;
                }
                else
                position.Y = position.Y + speed;

                correction = 0;
            }

            if (position.Y > endPosition.Y)
            {
                if (Math.Abs(position.Y - endPosition.Y) < speed)
                {
                    correction = (int)Math.Abs(position.Y - endPosition.Y);
                    position.Y = position.Y - correction;
                }
                else
                position.Y = position.Y - speed;

                correction = 0;
            }

        }

        public virtual void Update(float fps)
        {
            MovementAI();
            base.Update(fps);
        }

        public void OnImpact(Graphics dc)
        {
            dc.DrawImage(explosion, this.position.X, this.position.Y, explosion.Width, explosion.Height);
        }

    }
}
