using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViralTree.Components;

namespace ViralTree.Drawer
{
    public class MultiTextureDrawer : ADrawer
    {
        private List<TextureDrawer> drawers;

        public MultiTextureDrawer(params string[] pathes) 
        {
            drawers = new List<TextureDrawer>();

            for (int i = 0; i < pathes.Length; i++)
            {
                drawers.Add(new TextureDrawer(pathes[i]));
            }
        }

        public override void Initialize()
        {
            base.Initialize();

            for (int i = 0; i < drawers.Count; i++)
            {
                drawers[i].Owner = this.Owner;
                drawers[i].Initialize();
            
            }
        }


        public override void Draw(SFML.Graphics.RenderTarget target)
        {
            for (int i = 0; i < drawers.Count; i++)
            {
                drawers[i].Draw(target);
            }
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            for (int i = 0; i < drawers.Count; i++)
            {
                drawers[i].Update(gameTime, world);
            }
        }
    }
}
