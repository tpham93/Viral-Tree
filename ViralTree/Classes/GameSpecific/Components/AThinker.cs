using ViralTree.World;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public abstract class AThinker : AComponent
    {
        public abstract override void Update(GameTime gameTime, GameWorld world);
    }

    public sealed class EmptyThinker : AThinker
    {
        private static EmptyThinker instance;

        public static EmptyThinker Instance 
        {
            get { if (instance == null) instance = new EmptyThinker(); return instance; }
            private set { instance = value; }
        }

        private EmptyThinker()
        {

        }

        public override void Update(GameTime gameTime, GameWorld world)
        {

        }
    }
}
