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
            UpdateSprites();

            float life = Owner.CurrentLife / Owner.MaxLife;

            hpSprite.Scale = new Vector2f(life, life);
        }

        private void UpdateSprites()
        {
            bodySprite.Position = Owner.Collider.Position;
            hpSprite.Position = Owner.Collider.Position;
            specialSprite.Position = Owner.Collider.Position;

            bodySprite.Rotation = MathUtil.ToDegree(Owner.Collider.Rotation);
            hpSprite.Rotation = bodySprite.Rotation;
            specialSprite.Rotation = bodySprite.Rotation;
        }
    }
}
