using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Window;

namespace ViralTree.Components
{
    class CloudThinker : AThinker
    {
        TimeSpan leftDuration;
        TimeSpan duration;
        float minRadius;
        float maxRadius;

        public CloudThinker(TimeSpan duration, float minRadius, float maxRadius)
        {
            this.duration = duration;
            this.minRadius = minRadius;
            this.maxRadius = maxRadius;
            this.leftDuration = duration;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            leftDuration -= gameTime.ElapsedTime;
            Owner.CurrentLife = Owner.MaxLife * (float)(leftDuration.TotalMilliseconds / duration.TotalMilliseconds);
            Owner.Collider.Scale = 1 + (1-(Owner.CurrentLife / Owner.MaxLife)) * (maxRadius - minRadius) / minRadius;
        }
    }
}
