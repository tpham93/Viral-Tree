using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.Window;

namespace ViralTree.Components
{
    class TextureDrawer : ADrawer
    {
        Sprite sprite;
        public TextureDrawer(String filePath)
        {
            Texture texture = Game.content.Load<Texture>(filePath);
            sprite = new Sprite(texture);
            sprite.Origin = new Vector2f(sprite.TextureRect.Width / 2, sprite.TextureRect.Height/2);
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            sprite.Position = Owner.Collider.Position;
        }

        public override void Draw(SFML.Graphics.RenderTarget target)
        {
            target.Draw(sprite);
        }
    }
}
