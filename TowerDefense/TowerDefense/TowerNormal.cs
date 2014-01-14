using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TowerDefense
{
    public class TowerNormal : Tower
    {
        // Field

        // Properties

        /// <summary>
        /// Constructor - Creates a new projectile for this tower in the process
        /// Saves starting damage as well, to be able to restore if tower is no longer boosted
        /// </summary>
        /// <param name="projectileDamage"></param>
        /// <param name="speed"></param>
        /// <param name="cost"></param>
        /// <param name="range"></param>
        /// <param name="imagePath"></param>
        /// <param name="position"></param>
        public TowerNormal(int projectileDamage, float speed, int cost, float range, string imagePath, PointF position)
            : base(speed, cost, range, imagePath, position)
        {
            this.Bullet = new Projectile(projectileDamage, 20, @"Graphic/CannonBall.png", position, this);
            this.SavedDamage = this.Bullet.damage;
        }

        public override void Update(float FPS)
        {
            base.Update(FPS);
        }
    }
}
