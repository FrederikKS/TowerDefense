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
        private float speedOriginal;
        private int armor;
        private int worth;
        private Effect effect;
        private string name;
        private PointF endPosition;
        private int reachedPointCounter = 0;
        private int reachedEndCounter = 0;
        private bool enabled = false;
        private bool check = false;
        private Image explosion = Image.FromFile(@"Images/explosion.png");

        #endregion
        //properties
        #region

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
        public bool Check
        {
            get { return check; }
            set { check = value; }
        }
        public float SpeedOriginal
        {
            get { return speedOriginal; }
            set { speedOriginal = value; }
        }
        #endregion
        //constructor
        public Enemy(string name, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition)
            : base(imagePath, position)
        {
            this.name = name;
            this.hp = hp;
            this.speed = speed;
            this.speedOriginal = speed;
            this.armor = armor;
            this.worth = worth;
            this.effect = effect;
            this.endPosition = endPosition;
        }

        /// <summary>
        /// Frederik - Movement
        /// Tobias - Sprite update
        /// Moves enemy along path
        /// </summary>
        public void MovementAI()
        {
            PointF lastPos = new PointF(position.X, position.Y);
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
            EnemyEvade temp;
            // Right
            if (lastPos.X < position.X)
            {
                if (AnimationFrames.Count > 1)
                {
                    if (this is EnemySlow)
                    {
                        this.sprite = this.AnimationFrames[3];
                    }
                    if (this is EnemyEvade)
                    {
                        temp = (EnemyEvade)this;

                        if (!temp.IsEvade)
                            this.sprite = this.AnimationFrames[3];
                        else
                            this.sprite = this.AnimationFrames[7];

                        temp = null;
                    }
                    if (this is EnemyNormal)
                    {
                        this.sprite = this.AnimationFrames[3];
                    }
                }
            }

            //left
            if (lastPos.X > position.X)
            {
                if (AnimationFrames.Count > 1)
                {
                    if (this is EnemySlow)
                    {
                        this.sprite = this.AnimationFrames[1];
                    }
                    if (this is EnemyEvade)
                    {
                        temp = (EnemyEvade)this;

                        if (!temp.IsEvade)
                            this.sprite = this.AnimationFrames[1];
                        else
                            this.sprite = this.AnimationFrames[5];

                        temp = null;
                    }
                    if (this is EnemyNormal)
                    {
                        this.sprite = this.AnimationFrames[1];
                    }
                }
            }
            // Down
            if (lastPos.Y < position.Y)
            {
                if (AnimationFrames.Count > 1)
                {
                    if (this is EnemySlow)
                    {
                        this.sprite = this.AnimationFrames[2];
                    }
                    if (this is EnemyEvade)
                    {
                        temp = (EnemyEvade)this;
                        
                        if (!temp.IsEvade)
                            this.sprite = this.AnimationFrames[2];
                        else
                            this.sprite = this.AnimationFrames[6];

                        temp = null;
                    }
                    if (this is EnemyNormal)
                    {
                        this.sprite = this.AnimationFrames[2];
                    }
                }
            }
            // Up
            if (lastPos.Y > position.Y)
            {
                if (AnimationFrames.Count > 1)
                {
                    if (this is EnemySlow)
                    {
                        this.sprite = this.AnimationFrames[0];
                    }
                    if (this is EnemyEvade)
                    {
                        temp = (EnemyEvade)this;

                        if (!temp.IsEvade)
                            this.sprite = this.AnimationFrames[0];
                        else
                            this.sprite = this.AnimationFrames[4];

                        temp = null;
                    }
                    if (this is EnemyNormal)
                    {
                        this.sprite = this.AnimationFrames[0];
                    }
                }
            }

        }

        public virtual void Update(float fps)
        {
            MovementAI();
            base.Update(fps);
        }

        /// <summary>
        /// Performs this action when enemy is hit with a projectile. Will be used for effects
        /// </summary>
        public void OnImpact()
        {
            //dc.DrawImage(explosion, this.position.X, this.position.Y, explosion.Width, explosion.Height);
        }
        
    }
}
