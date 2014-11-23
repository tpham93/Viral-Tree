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
        private float scale;

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

             scale = (float)Math.Pow(Math.Sin(gameTime.TotalTime.TotalSeconds), 2) * 0.2f + 0.8f;
          //  Console.WriteLine(this.Owner.Collider.Scale);

            playerSprite.Position = Owner.Collider.Position;
            playerSprite.Rotation = Owner.Collider.Rotation;
            nucleusSprite.Position = playerSprite.Position;
            float life = Owner.CurrentLife / Owner.MaxLife;
            healthSprite.Position = playerSprite.Position;
            Vector2f healthSpriteScale = new Vector2f(life * scale, life * scale);
            healthSprite.Scale = new Vector2f((float)Math.Min(healthSpriteScale.X, 1), (float)Math.Min(healthSpriteScale.Y, 1));
            mitochondrionSprite.Position = playerSprite.Position;
            mitochondrionOffset = (float)gameTime.TotalTime.TotalSeconds * 0.1f * (float)Math.PI;
        }

        public override void Draw(RenderTarget target)
        {
            float directionAngle =  MathUtil.ToDegree(Vec2f.RotationFrom(Owner.Collider.Direction));

          //  playerSprite.Scale = new Vector2f(Owner.Collider.Scale, Owner.Collider.Scale);
         //   nucleusSprite.Scale = new Vector2f(Owner.Collider.Scale, Owner.Collider.Scale);
           // healthSprite.Scale = new Vector2f(scale, scale);

            playerSprite.Rotation =  directionAngle;
            nucleusSprite.Rotation = directionAngle;
            healthSprite.Rotation = directionAngle;

            target.Draw(playerSprite);
            target.Draw(nucleusSprite);
            target.Draw(healthSprite);


            for (int i = 0; i < Math.Ceiling(mitochondrionsNum); ++i)
            {
                float angle = MathUtil.ToDegree((i / (float)Math.Ceiling(mitochondrionsNum)) * MathUtil.PI * 2.0f + mitochondrionOffset);
               // mitochondrionSprite.Scale = new Vector2f(Owner.Collider.Scale, Owner.Collider.Scale);
                mitochondrionSprite.Rotation = angle;
                target.Draw(mitochondrionSprite);
            }
        }
    }
}
