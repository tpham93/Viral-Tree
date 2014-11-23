using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;
using ViralTree.World;

namespace ViralTree.Components
{
    class CollectibleResponse : ACollisionResponse
    {
        EntityType type;
        float value;
        

        public CollectibleResponse(EntityType type ,float value)
        {
            this.type = type;
            this.value = value;
        }

        public override void OnCollision(Entity collidedEntity, IntersectionData data, GameWorld world, bool firstCalled, GameTime gameTime)
        {
            if (collidedEntity.Fraction == Fraction.Cell)
            {
                if (type == EntityType.Health)
                    collidedEntity.CurrentLife = Math.Min((byte)collidedEntity.MaxLife, (byte)collidedEntity.CurrentLife + value);

                Owner.CurrentLife = 0;
            }        
        }
        public override void Update(GameTime gameTime, GameWorld world)
        {
        }
    }
}
