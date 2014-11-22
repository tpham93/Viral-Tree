using ViralTree.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;

namespace ViralTree.Components
{
    public abstract class ACollisionResponse : AComponent
    {
        /// <summary> Indicates whether the Response is pushable (will be pushed on collision) or not (wont be pushed, but can push) </summary>
        public bool isPushable;

        /// <summary> Indicates whether this Response even cause pushes.</summary>
        public bool isPushing;

     

        public abstract void OnCollision(Entity collidedEntity, IntersectionData data, GameWorld world, bool firstCalled, GameTime gameTime);

        public abstract override void Update(GameTime gameTime, GameWorld world);
    }

    public sealed class EmptyResponse : ACollisionResponse
    {
        private static EmptyResponse instance;

        public static EmptyResponse Instance
        {
            get { if (instance == null) instance = new EmptyResponse(); return instance; }
            private set { instance = value; }
        }

        private EmptyResponse() 
        {
            isPushable = false;
            isPushing = false;
         
        }


        public override void OnCollision(Entity collidedEntity, IntersectionData data, GameWorld world, bool firstCalled, GameTime gameTime)
        {

        }

        public override void Update(GameTime gameTime, GameWorld world)
        {
            
        }
    }
}
