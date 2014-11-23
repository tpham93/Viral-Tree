using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.World;

namespace ViralTree.Components
{
    public class TouchDamageResponse : ACollisionResponse
    {
        BasicPushResponse pushResponse;

        Fraction fraction;
        float damage;

        public TouchDamageResponse(Fraction fraction, float damage, bool pushable, bool isPushing)
        {
            pushResponse = new BasicPushResponse(pushable);
            pushResponse.isPushable = isPushing;

            this.isPushable = pushable;
            this.isPushing = isPushing;

            this.fraction = fraction;
            this.damage = damage;
        }

        public override void Initialize()
        {
            pushResponse.Initialize();
            pushResponse.Owner = this.Owner;
         
        }

        public override void OnCollision(World.Entity collidedEntity, IntersectionData data, World.GameWorld world, bool firstCalled, GameTime gameTime)
        {
            pushResponse.OnCollision(collidedEntity, data, world, firstCalled, gameTime);

            float tmpDamage = damage * (float)gameTime.ElapsedTime.TotalSeconds;
          

            if (fraction == Fraction.Neutral && (collidedEntity.Fraction == Fraction.Virus || collidedEntity.Fraction == Fraction.Cell))
                collidedEntity.CurrentLife -= tmpDamage;

            else if (fraction == Fraction.Virus && collidedEntity.Fraction == Fraction.Cell)
                collidedEntity.CurrentLife -= tmpDamage;

            else if (fraction == Fraction.Cell && collidedEntity.Fraction == Fraction.Virus)
                collidedEntity.CurrentLife -= tmpDamage;

            else if (fraction == Fraction.CellProjectile && collidedEntity.Fraction == Fraction.Virus)
                collidedEntity.CurrentLife -= tmpDamage;

            else if (fraction == Fraction.VirusProjectile && collidedEntity.Fraction == Fraction.Cell)
                collidedEntity.CurrentLife -= tmpDamage;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            
        }
    }
}
