using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViralTree.World;

namespace ViralTree.Components
{
    class ShieldThinker : AThinker
    {
        Entity target;
        float shieldDist;
        TimeSpan duration;
        public ShieldThinker(Entity target, float shieldDist, TimeSpan duration)
        {
            this.target = target;
            this.shieldDist = shieldDist;
            this.duration = duration;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            duration -= gameTime.ElapsedTime;
            Owner.Collider.Position = target.Collider.Position + target.Collider.Direction * shieldDist;
            Owner.Collider.Rotation = target.Collider.Rotation;

            if(duration<TimeSpan.Zero)
            {
                Owner.CurrentLife = 0;
            }
        }
    }
}
