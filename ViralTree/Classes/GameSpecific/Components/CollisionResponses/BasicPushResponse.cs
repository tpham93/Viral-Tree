using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public sealed class BasicPushResponse : ACollisionResponse
    {
        public BasicPushResponse(bool pushable)
        {
            isPushable = pushable;

            isPushing = true;
        }

        public override void OnCollision(World.Entity collidedEntity, IntersectionData data, World.GameWorld world, bool firstCalled)
        {
            if (collidedEntity.Response.isPushing)
            {
                if (Owner.Response.isPushable)
                {
                    if (collidedEntity.Response.isPushable)
                    {
                        data.Seperate(this.Owner.Collider, firstCalled ? 0.5f : -0.5f);
                    }


                    else
                    {
                        data.Seperate(this.Owner.Collider, firstCalled ? 1.0f : -1.0f);
                    }

                    //TODO: add?
                    //Owner.LeftCurrentChunk(world);

                }

            }
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            
        }
    }
}
