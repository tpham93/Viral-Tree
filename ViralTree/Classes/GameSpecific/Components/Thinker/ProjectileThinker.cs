using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Window;

namespace ViralTree.Components
{
    class ProjectileThinker : AThinker
    {
        Vector2f direction;
        float speed;
        ProjectileThinker(Vector2f direction, float speed )
        {
            this.direction = direction;
            this.speed = speed;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            Owner.Collider.Position += direction * speed * (float)gameTime.ElapsedTime.TotalSeconds;
        }
    }
}
