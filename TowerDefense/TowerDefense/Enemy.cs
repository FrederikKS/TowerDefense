using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Diagnostics;
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
        private bool check = false;
        private bool stun = false;
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
        public bool Check
        {
            get { return check; }
            set { check = value; }
        }
        #endregion
        //constructor
        public Enemy(string name, float hp, float speed, int armor, int worth, Effect effect, string imagePath, PointF position, PointF endPosition, bool isClickable)
            : base(imagePath, position, isClickable)
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
            // Right
            if (lastPos.X < position.X)
                if (AnimationFrames.Count > 1)
                    this.sprite = Image.FromFile(@"Graphic/Resized/SlowResizedRight.png");
            //Right
            if (lastPos.X > position.X)
                if (AnimationFrames.Count > 1)
                    this.sprite = Image.FromFile(@"Graphic/Resized/SlowResizedLeft.png");
            // Up
            if (lastPos.Y < position.Y)
                if (AnimationFrames.Count > 1)
                    this.sprite = Image.FromFile(@"Graphic/Resized/SlowResizedDown.png");
            // Down
            if (lastPos.Y > position.Y)
                if (AnimationFrames.Count > 1)
                    this.sprite = Image.FromFile(@"Graphic/Resized/SlowResized.png");

        }

        public virtual void Update(float fps)
        {
            MovementAI();
            base.Update(fps);
        }

        public void OnImpact(Graphics dc)
        {
            //dc.DrawImage(explosion, this.position.X, this.position.Y, explosion.Width, explosion.Height);
        }
        //public override void UpdateAnimation(float fps)
        //{
        //    base.UpdateAnimation(fps);
        //    // Left
        //    if (lastPos.X < position.X)
        //    {
        //        if (AnimationFrames.Count > 1)
        //        {
        //            if (this is EnemyEvade)
        //            {
        //                this.sprite = Form1.gw.eLeft[1];
        //                this.sprite = AnimationFrames[1];
        //            }
        //            else if (this is EnemySlow)
        //            {
        //                this.sprite = Form1.gw.eLeft[0];
        //                this.sprite = AnimationFrames[1];
        //            }
        //        }
        //    }
        //    // Right
        //    if (lastPos.X > position.X)
        //    {
        //        if (AnimationFrames.Count > 1)
        //        {
        //            if (this is EnemyEvade)
        //            {
        //                this.sprite = Form1.gw.eRight[1];
        //                this.sprite = AnimationFrames[3];
        //            }
        //            else if (this is EnemySlow)
        //            {
        //                this.sprite = Form1.gw.eRight[0];
        //                this.sprite = AnimationFrames[3];
        //            }
        //        }
        //    }
        //    // Up
        //    if (lastPos.Y < position.Y)
        //    {
        //        if (AnimationFrames.Count > 1)
        //        {
        //            if (this is EnemyEvade)
        //            {
        //                this.sprite = Form1.gw.eTop[1];
        //                this.sprite = AnimationFrames[0];
        //            }
        //            else if (this is EnemySlow)
        //            {
        //                this.sprite = Form1.gw.eTop[0];
        //                this.sprite = AnimationFrames[0];
        //            }
        //        }
        //    }
        //    // Down
        //    if (lastPos.Y > position.Y)
        //    {
        //        if (AnimationFrames.Count > 1)
        //        {
        //            if (this is EnemyEvade)
        //            {
        //                this.sprite = Form1.gw.eBot[1];
        //                this.sprite = AnimationFrames[2];
        //            }
        //            else if (this is EnemySlow)
        //            {
        //                this.sprite = Form1.gw.eBot[0];
        //                this.sprite = AnimationFrames[2];
        //            }
        //        }
        //    }
        //}
    }
}
