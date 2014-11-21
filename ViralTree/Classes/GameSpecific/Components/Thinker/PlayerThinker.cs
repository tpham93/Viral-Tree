using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public sealed class PlayerThinker : AThinker
    {
        private float speed = 5.0f;

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            if (KInput.IsPressed(SFML.Window.Keyboard.Key.W))
                Owner.Collider.Move(0, -speed);

            else if (KInput.IsPressed(SFML.Window.Keyboard.Key.S))
                    Owner.Collider.Move(0, speed);

            if (KInput.IsPressed(SFML.Window.Keyboard.Key.A))
                Owner.Collider.Move(-speed , 0);

            else if (KInput.IsPressed(SFML.Window.Keyboard.Key.D))
                Owner.Collider.Move(speed, 0);


            if (KInput.IsPressed(SFML.Window.Keyboard.Key.Q))
                Owner.Collider.Rotate(-0.1f);

            else if (KInput.IsPressed(SFML.Window.Keyboard.Key.E))
                Owner.Collider.Rotate(0.1f);

            if (KInput.IsClicked(SFML.Window.Keyboard.Key.Space))
                Owner.Activator.Activate(gameTime, world);


            /*
            Vector2f mousePos = MInput.GetMousePos(new Vector2f(world.Cam.Position.X - Settings.WindowSize.X * 0.5f, world.Cam.Position.Y - Settings.WindowSize.Y * 0.5f));
            Logger.Write(world.Cam.currentView.Viewport);
            Vector2f dir = mousePos - Owner.Collider.Position;
            dir = Vec2f.Normalized(dir);

            Owner.Collider.Rotation = Vec2f.RotationFrom(dir);
            */
        }
    }
}
