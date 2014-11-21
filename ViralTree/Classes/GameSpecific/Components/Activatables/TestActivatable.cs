using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public sealed class TestActivatable : AActivatable
    {
        private bool activated = false;

        public TestActivatable(float distance)
        {
            this.ActivateDistance = distance;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            if (activated)
            {
                Owner.Collider.Rotation = (float)Math.Cos(gameTime.TotalTime.TotalSeconds) * MathUtil.TWO_PI;
            }
        }

        public override void OnActivation(GameTime gameTime, World.GameWorld world, World.Entity activator)
        {
            activated = !activated;
        }
    }
}
