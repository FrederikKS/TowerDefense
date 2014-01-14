using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public abstract class GameObject
    {
        // test
        //Fields
        #region
        public bool isClickable;
        protected PointF position;
        public Image sprite;
        private List<Image> animationFrames;
        public float currentFrameIndex;
        private RectangleF collisionRect;
        private PointF lastPos = new PointF(0, 0);

        #endregion

        //Properties
        #region Property
        public PointF Position 
        {
            get { return position; }
            set { position = value; }
        }
        public RectangleF CollisionRect
        {
            get { return collisionRect; }
            set { collisionRect = value; }
        }
        public Image Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }
        public List<Image> AnimationFrames
        {
            get { return animationFrames; }
            set { animationFrames = value; }
        }
        #endregion

        /// <summary>
        /// GameObject constructor
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="Position"></param>
        /// <param name="isClickable"></param>
        public GameObject(string imagePath, PointF Position)
        {
            // Seperates the image path on "," enables multiple image paths
            string[] imagePaths = imagePath.Split(new char[] { ',' });
            this.Position = Position;
            this.animationFrames = new List<Image>();

            // Loop through imagepaths and add them to animation frames

            foreach (string path in imagePaths)
            {
                animationFrames.Add(Image.FromFile(path));
            }

            //Set the sprite to the first image in the animation frames
            this.sprite = this.animationFrames[0];
        }
        
        //Functions
        #region Functions
        /// <summary>
        /// GameObject Update
        /// </summary>
        /// <param name="fps"></param>
        public virtual void Update(float fps)
        {
            
        }

        /// <summary>
        /// Animation update
        /// </summary>
        /// <param name="fps"></param>
        public virtual void UpdateAnimation(float fps)
        {
            //Calculates the current index, so that the amount of images is spread out on one 1 second 
            float factor = 1 / fps;
            currentFrameIndex += factor * animationFrames.Count();
            //If currentAnimationIndex is outside of amount of images, reset it to the first image

            if (currentFrameIndex >= animationFrames.Count())
            {
                currentFrameIndex = 0;
            }

            //Convert currentAnimationIndex to a integer (removing everything after the dot) and used it to 
            //get the correct image
            
            // sprite = animationFrames[(int)currentFrameIndex];

            collisionRect = new RectangleF(Position.X, Position.Y, sprite.Width, sprite.Height);
        }

        /// <summary>
        /// Draw function
        /// </summary>
        /// <param name="dc"></param>
        public virtual void Draw(Graphics dc)
        {
            dc.DrawImage(sprite,position.X, position.Y, 96, 96);
            Pen pen = new Pen(Color.Red);
#if DEBUG
            dc.DrawRectangle(pen, collisionRect.X, collisionRect.Y, collisionRect.Width = 96, collisionRect.Height = 96);
#endif
        }

        #endregion
    }
}
