using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public class TankDrawer : ADrawer
    {
        Sprite bodySprite;
        Sprite hpSprite;
        Sprite specialSprite;

        private float scale;

        public TankDrawer()
        {
            bodySprite = new Sprite(Game.content.Load<Texture>("gfx/Player/Tank/tankBody.png"));
            bodySprite.Origin = new Vector2f(bodySprite.Texture.Size.X * 0.5f, bodySprite.Texture.Size.Y * 0.5f);

            hpSprite = new Sprite(Game.content.Load<Texture>("gfx/Player/Tank/tankLife.png"));
            hpSprite.Origin = bodySprite.Origin;

            specialSprite = new Sprite(Game.content.Load<Texture>("gfx/Player/Tank/tankSpecial.png"));
            specialSprite.Origin = bodySprite.Origin;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Draw(SFML.Graphics.RenderTarget target)
        {

          

            target.Draw(bodySprite);
            target.Draw(hpSprite);
            target.Draw(specialSprite);
            

            //Owner.Collider.Draw(target);
          
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            scale = (float)Math.Pow(Math.Sin(gameTime.TotalTime.TotalSeconds), 2) * 0.2f + 0.8f;

            UpdateSprites();

            float life = Owner.CurrentLife / Owner.MaxLife;

            Vector2f healthSpriteScale = new Vector2f(life * scale, life * scale);
            hpSprite.Scale = new Vector2f((float)Math.Min(healthSpriteScale.X, 1), (float)Math.Min(healthSpriteScale.Y, 1));

          //  bodySprite.Scale = new Vector2f(0.1f + scale, 0.1f + scale);
        }

        private void UpdateSprites()
        {
           // hpSprite.Scale = new Vector2f(this.Owner.Collider.Scale, this.Owner.Collider.Scale);
          //  hpSprite.Scale = new Vector2f(scale, scale);

            bodySprite.Position = Owner.Collider.Position;
            hpSprite.Position = Owner.Collider.Position;
            specialSprite.Position = Owner.Collider.Position;

            bodySprite.Rotation = MathUtil.ToDegree(Owner.Collider.Rotation);
            hpSprite.Rotation = bodySprite.Rotation;
            specialSprite.Rotation = bodySprite.Rotation;
        }
    }
}
