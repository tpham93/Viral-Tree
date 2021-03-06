﻿using SFML.Graphics;
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

        private AWeapon specialWeapon;

        public TankDrawer(AWeapon specialWeapon)
        {
            this.specialWeapon = specialWeapon;

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

            //Owner.Collider.Draw(target);
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {

            specialSprite.Color = new Color(255,255,255,(byte)(255*(1-specialWeapon.CoolDown.TotalMilliseconds / specialWeapon.MaxCoolDown.TotalMilliseconds)));

            scale = (float)Math.Pow(Math.Sin(gameTime.TotalTime.TotalSeconds), 2) * 0.2f + 0.8f;

           // float scale2 = (float)Math.Pow(Math.Sin(gameTime.TotalTime.TotalSeconds), 2) * 0.15f + 0.95f;

            UpdateSprites();

            float life = Owner.CurrentLife / Owner.MaxLife;

            Vector2f healthSpriteScale = new Vector2f(life * scale, life * scale);
            hpSprite.Scale = new Vector2f((float)Math.Min(healthSpriteScale.X, 1), (float)Math.Min(healthSpriteScale.Y, 1));

            /*
            bodySprite.Scale = new Vector2f(scale2, scale2);
            specialSprite.Scale = bodySprite.Scale;
             */
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
