using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public class KeyResponse : ACollisionResponse
    {

        public ExitResponse connectedExit;

        public KeyResponse(ExitResponse response)
        {
            this.connectedExit = response;
        }

        public override void OnCollision(World.Entity collidedEntity, IntersectionData data, World.GameWorld world, bool firstCalled, GameTime gameTime)
        {
           
            if (connectedExit != null && (collidedEntity == connectedExit.player1 || collidedEntity == connectedExit.player2))
            {
                connectedExit.numKeys--;
                Owner.CurrentLife = 0.0f;
            }

            
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
           
        }
    }
}
