using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace ViralTree.Components
{
    class PlayerDrawer : ADrawer
    {
        private Sprite playerSprite;
        private Sprite nucleusSprite;
        private Sprite healthSprite;
        private Texture mitochondrionTexture;
        private Sprite mitochondrionSprite;
        private Vector2f[] mitochondrions;

        public PlayerDrawer()
        {
        }

        public override void LoadContent(ContentManager contentManager)
        {
            Texture playerTexture = contentManager.Load<Texture>("gfx/Player/player.png");
            Texture nucleusTexture = contentManager.Load<Texture>("gfx/Player/nucleus.png");
            Texture healthTexture = contentManager.Load<Texture>("gfx/Player/health.png");
            mitochondrionTexture = contentManager.Load<Texture>("gfx/Player/mitochondrion.png");

            playerSprite = new Sprite(playerTexture);
            nucleusSprite = new Sprite(nucleusTexture);
            healthSprite = new Sprite(healthTexture);
            mitochondrionSprite = new Sprite(mitochondrionTexture);
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            playerSprite.Position = Owner.Collider.Position;
            playerSprite.Rotation = Owner.Collider.Rotation;
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(playerSprite);
        }
    }
}
