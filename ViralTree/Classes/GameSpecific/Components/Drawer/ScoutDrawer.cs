using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.Window;

namespace ViralTree.Components
{
    class ScoutDrawer : ADrawer
    {
        private Sprite playerSprite;
        private Sprite nucleusSprite;
        private Sprite healthSprite;
        private Texture mitochondrionTexture;
        private Sprite mitochondrionSprite;
        private float mitochondrionsNum;

        private float mitochondrionOffset;

        public ScoutDrawer()
        {
            mitochondrionsNum = 5.5f;

            Texture playerTexture = Game.content.Load<Texture>("gfx/Player/Scout/player.png");
            Texture nucleusTexture = Game.content.Load<Texture>("gfx/Player/Scout/nucleus.png");
            Texture healthTexture = Game.content.Load<Texture>("gfx/Player/Scout/health.png");
            mitochondrionTexture = Game.content.Load<Texture>("gfx/Player/Scout/mitochondrion.png");

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
            mitochondrionSprite.Position = playerSprite.Position;
            mitochondrionOffset = (float)gameTime.TotalTime.TotalSeconds * 0.1f * (float)Math.PI;
        }

        public override void Draw(RenderTarget target)
        {
            target.Draw(playerSprite);
            target.Draw(nucleusSprite);
            target.Draw(healthSprite);

            for (int i = 0; i < Math.Ceiling(mitochondrionsNum); ++i)
            {
                float angle = MathUtil.ToDegree((i / (float)Math.Ceiling(mitochondrionsNum)) * MathUtil.PI * 2.0f + mitochondrionOffset);
                mitochondrionSprite.Rotation = angle;
                target.Draw(mitochondrionSprite);
            }
        }
    }
}
