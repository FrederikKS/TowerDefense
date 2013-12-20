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
        protected List<Image> animationFrames;
        protected float currentFrameIndex;
        private RectangleF collisionRect;

        

        #endregion

        //Properties
        #region
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
        #endregion

        /// <summary>
        /// GameObject constructor
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="Position"></param>
        /// <param name="isClickable"></param>
        public GameObject(string imagePath, PointF Position, bool isClickable)
        {

            // Seperates the image path on ; enables multiple image paths

            string[] imagePaths = imagePath.Split(new char[] { ',' });
            this.Position = Position;
            sprite = Image.FromFile(imagePaths[0]);
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
        #region
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
            // (1 / fps) * amount of images

            float factor = 1 / fps;
            currentFrameIndex += factor * animationFrames.Count();
            //If currentAnimationIndex is outside of amount of images, reset it to the first image

            if (currentFrameIndex >= animationFrames.Count())
            {
                currentFrameIndex = 0;
            }

            //Convert currentAnimationIndex to a integer (removing everything after the dot) and used it to 
            //get the correct image

            sprite = animationFrames[(int)currentFrameIndex];
            collisionRect = new RectangleF(Position.X, Position.Y, sprite.Width, sprite.Height);


        }

        /// <summary>
        /// Draw function
        /// </summary>
        /// <param name="dc"></param>
        public virtual void Draw(Graphics dc)
        {
            dc.DrawImage(sprite,position.X, position.Y,100, 100);
            
        }

        #endregion
    }
}
