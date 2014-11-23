using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.World;

namespace ViralTree.Components
{
    public class ExitResponse : ACollisionResponse
    {
        public Entity player1;
        public Entity player2;

        public int numKeys;

        public ExitResponse(int numKeys, Entity player1, Entity player2)
        {
            this.player1 = player1;
            this.player2 = player2;

            this.numKeys = numKeys;
        }

        public override void OnCollision(World.Entity collidedEntity, IntersectionData data, World.GameWorld world, bool firstCalled, GameTime gameTime)
        {
            if (numKeys <= 0 && (player1 != null && collidedEntity == player1 || player2 != null && collidedEntity == player2))
            {
                
            }
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            
        }
    }
}
