using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public class KeyResponse : ACollisionResponse
    {

        ExitResponse connectedExit;

        public KeyResponse(ExitResponse response)
        {
            this.connectedExit = response;
        }

        public override void OnCollision(World.Entity collidedEntity, IntersectionData data, World.GameWorld world, bool firstCalled, GameTime gameTime)
        {
            if (collidedEntity == connectedExit.player1 || collidedEntity == connectedExit.player2)
            {
                connectedExit.numKeys--;
                world.QueueRemovingEntity(this.Owner);
            }
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
           
        }
    }
}
