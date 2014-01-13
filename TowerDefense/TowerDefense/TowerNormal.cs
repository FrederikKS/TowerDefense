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

        public TowerNormal(int projectileDamage, float speed, int cost, float range, string imagePath, PointF position, bool isClickAble)
            : base(speed, cost, range, imagePath, position, isClickAble)
        {
            this.Bullet = new Projectile(projectileDamage, 20, @"Graphic/CannonBall.png", position, false, this);
            this.SavedDamage = this.Bullet.damage;
        }

        public override void Update(float FPS)
        {
            base.Update(FPS);
        }
    }
}
