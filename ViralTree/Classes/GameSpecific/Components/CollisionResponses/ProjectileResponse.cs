using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.World;

namespace ViralTree.Components
{
    class ProjectileResponse : ACollisionResponse
    {
        float damage;
        public ProjectileResponse(float damage)
        {
            this.damage = damage;
        }

        public override void OnCollision(Entity collidedEntity, IntersectionData data, GameWorld world, bool firstCalled)
        {
            if (Entity.CanCollide(Owner.Fraction, collidedEntity.Fraction, collidedEntity.CollidingFraction) || Entity.CanCollide(collidedEntity.Fraction, Owner.Fraction, Owner.CollidingFraction))
            {
                collidedEntity.CurrentLife -= this.damage;
                this.Owner.CurrentLife = 0;
            }
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
        }
    }
}
