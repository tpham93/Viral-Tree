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

        private double startTime;

        public KeyResponse(ExitResponse response, double startTime)
        {
            this.connectedExit = response;

            

            this.startTime = startTime;
        }

        public override void Initialize()
        {
            base.Initialize();
            this.Owner.Drawable = false;
        }

        public override void OnCollision(World.Entity collidedEntity, IntersectionData data, World.GameWorld world, bool firstCalled, GameTime gameTime)
        {
           
            if (startTime <= 0 && connectedExit != null && (collidedEntity == connectedExit.player1 || collidedEntity == connectedExit.player2))
            {
                connectedExit.RemoveNumKey(this);
                Owner.CurrentLife = 0.0f;


                if (connectedExit.player1 != null)
                    connectedExit.player1.CurrentLife += 50.0f;

                if (connectedExit.player2 != null)
                    connectedExit.player2.CurrentLife += 50.0f;
            }

           

            
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            if (startTime >= 0)
                startTime -= gameTime.ElapsedTime.TotalSeconds;

            if (startTime <= 0)
            {
                Owner.Drawable = true;
            }
        }
    }
}
