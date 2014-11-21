using ViralTree.World;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public abstract class AActivator : AComponent
    {
        /// <summary>
        /// Use this method for example for cooldown.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="world"></param>
        public abstract override void Update(GameTime gameTime, World.GameWorld world);


        /// <summary>
        /// Just call InternActivate in derived class if necessary.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="world"></param>
        public abstract void Activate(GameTime gameTime, GameWorld world);

        //store all activated entities into the list 
        protected void InternActivate(GameTime gameTime, GameWorld world)
        {
            world.ActivateEntity(gameTime, Owner);
        }

        
    }

    public sealed class EmptyActivator : AActivator
    {
        private static EmptyActivator instance;
        public static EmptyActivator Instance { get { if (instance == null) instance = new EmptyActivator(); return instance; } private set { instance = value; } }

        private EmptyActivator()
        {

        }

        public override void Update(GameTime gameTime, GameWorld world)
        {
            
        }

        public override void Activate(GameTime gameTime, GameWorld world)
        {
           
        }
    }
}
