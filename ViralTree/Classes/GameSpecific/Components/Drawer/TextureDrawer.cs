﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML.Graphics;
using SFML.Window;

namespace ViralTree.Components
{
    public class TextureDrawer : ADrawer
    {
        public Sprite sprite;

        public bool useRed = true;

        public TextureDrawer(String filePath)
        {
            Texture texture = Game.content.Load<Texture>(filePath);
            sprite = new Sprite(texture);
            sprite.Origin = new Vector2f(sprite.TextureRect.Width / 2, sprite.TextureRect.Height/2);
        }

        public override void Initialize()
        {
            sprite.Position = Owner.Collider.Position;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            this.sprite.Scale = new Vector2f(Owner.Collider.Scale, Owner.Collider.Scale);
            sprite.Position = Owner.Collider.Position;
            sprite.Rotation = MathUtil.ToDegree(Owner.Collider.Rotation);

            if (useRed)
            {
                byte red = (byte)((Owner.Thinker.nextAttack()) * 255);
                sprite.Color = new Color(255, red, red, 255);
            }
 
        }

        public override void Draw(SFML.Graphics.RenderTarget target)
        {
            target.Draw(sprite);

          //  Owner.Collider.Draw(target);
        }
    }
}
