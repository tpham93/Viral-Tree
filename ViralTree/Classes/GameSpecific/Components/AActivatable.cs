using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public abstract class AActivatable : AComponent
    {
        public float ActivateDistance
        {
            get;
            set;
        }

        public abstract override void Update(GameTime gameTime, World.GameWorld world);

        public abstract void OnActivation(GameTime gameTime, World.GameWorld world, World.Entity activator);
    }

    public sealed class EmptyActivatable : AActivatable
    {

        private static AActivatable instance;
        public static AActivatable Instance { get { if (instance == null)instance = new EmptyActivatable(); return instance; } private set { instance = value; } }

        private EmptyActivatable()
        {
            ActivateDistance = float.PositiveInfinity;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            
        }

        public override void OnActivation(GameTime gameTime, World.GameWorld world, World.Entity activator)
        {
            
        }

    }
}
