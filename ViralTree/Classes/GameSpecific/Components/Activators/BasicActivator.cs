using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public sealed class BasicActivator : AActivator
    {
        public BasicActivator()
        {

        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            
        }

        public override void Activate(GameTime gameTime, World.GameWorld world)
        {
            InternActivate(gameTime, world);
        }
    }
}
