using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    class CloudDrawer : TextureDrawer
    {
        float minRadius;
        float maxRadius;
        public CloudDrawer(float minRadius, float maxRadius, String filePath)
            :base(filePath)
        {
            this.minRadius = minRadius;
            this.maxRadius = maxRadius;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            base.Update(gameTime, world);
            Owner.Collider.Scale = 1 + (Owner.CurrentLife / Owner.MaxLife) * (maxRadius - minRadius);
        }

        public override void Draw(SFML.Graphics.RenderTarget target)
        {
            base.Draw(target);
        }

    }
}
