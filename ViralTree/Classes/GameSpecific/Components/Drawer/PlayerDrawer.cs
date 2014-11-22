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
            mitochondrions = new Vector2f[5];
            float dist = 50;
            for(int i = 0; i < mitochondrions.Length; ++i)
            {
                float angle = i / (float)Math.PI * 1.5f;
                float x = (float)Math.Sin(angle);
                float y = (float)Math.Cos(angle);
                mitochondrions[i] = new Vector2f(x,y) * dist;
            }

            Texture playerTexture = Game.content.Load<Texture>("gfx/Player/player.png");
            Texture nucleusTexture = Game.content.Load<Texture>("gfx/Player/nucleus.png");
            Texture healthTexture = Game.content.Load<Texture>("gfx/Player/health.png");
            mitochondrionTexture = Game.content.Load<Texture>("gfx/Player/mitochondrion.png");

            playerSprite = new Sprite(playerTexture);
            nucleusSprite = new Sprite(nucleusTexture);
            healthSprite = new Sprite(healthTexture);
            mitochondrionSprite = new Sprite(mitochondrionTexture);

            playerSprite.Origin = new Vector2f(playerSprite.TextureRect.Width, playerSprite.TextureRect.Height) / 2.0f;
            nucleusSprite.Origin = new Vector2f(nucleusSprite.TextureRect.Width, nucleusSprite.TextureRect.Height) / 2.0f;
            healthSprite.Origin = new Vector2f(healthSprite.TextureRect.Width, healthSprite.TextureRect.Height) / 2.0f;
            mitochondrionSprite.Origin = new Vector2f(mitochondrionSprite.TextureRect.Width, mitochondrionSprite.TextureRect.Height) / 2.0f;
        }

        public override void Initialize()
        {
            base.Initialize();
            playerSprite.Position = Owner.Collider.Position;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            playerSprite.Position = Owner.Collider.Position;
            playerSprite.Rotation = Owner.Collider.Rotation;
            nucleusSprite.Position = playerSprite.Position;
            float life = Owner.CurrentLife / Owner.MaxLife;
            healthSprite.Position = playerSprite.Position;
            healthSprite.Scale = new Vector2f(life, life);
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(playerSprite);
            target.Draw(nucleusSprite);
            target.Draw(healthSprite);
            
            for(int i = 0; i < mitochondrions.Length; ++i)
            {
                mitochondrionSprite.Position = mitochondrions[i] + playerSprite.Position;
                target.Draw(mitochondrionSprite);
            }

        //Owner.Collider.Draw(target);
        }
    }
}
