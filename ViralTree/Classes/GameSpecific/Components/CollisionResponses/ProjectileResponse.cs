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
        bool pushBack;
        public ProjectileResponse(float damage, bool pushBack = false)
        {
            this.damage = damage;
            this.pushBack = pushBack;
        }

        public override void OnCollision(Entity collidedEntity, IntersectionData data, GameWorld world, bool firstCalled, GameTime gameTime)
        {
            if (Entity.CanCollide(Owner.Fraction, collidedEntity.Fraction, Owner.CollidingFraction, collidedEntity.CollidingFraction))
            {
                if (pushBack)
                {
                    if (collidedEntity.Fraction != Fraction.Neutral )
                    {
                        data.Seperate(collidedEntity.Collider, firstCalled ? -1.0f : 1.0f);
                    }
                }
                collidedEntity.CurrentLife -= this.damage;
                this.Owner.CurrentLife--;
            }
            if(collidedEntity.Fraction == Fraction.VirusProjectile)
            {
                collidedEntity.CurrentLife = 0.0f;
            }
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
        }
    }
}
