using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;

namespace ViralTree.Components
{
    public abstract class ADrawer : AComponent
    {
        public abstract void LoadContent(ContentManager contentmanager);
        public abstract void Draw(RenderTarget target);
    }

    public class EmptyDraw : ADrawer
    {
        public EmptyDraw()
        {

        }

        public override void LoadContent(ContentManager contentmanager)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(RenderTarget target)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
           // throw new NotImplementedException();
        }
    }
}
