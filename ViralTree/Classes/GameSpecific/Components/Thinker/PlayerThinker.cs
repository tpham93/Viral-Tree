﻿using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree.Components
{
    public sealed class PlayerThinker : AThinker
    {
        private GInput controller;
        private ShooterThinker weapon;

        public PlayerThinker(GInput controller = null)
        {
            this.controller = controller;
            weapon = new ShooterThinker(30, TimeSpan.FromMilliseconds(500.0f), new CircleCollider(16), float.PositiveInfinity);
        }

        public override void Initialize()
        {
            base.Initialize();

            weapon.Owner = this.Owner;
        }

        private PlayerInput GetInput()
        {
            PlayerInput input = new PlayerInput();
            Vector2f movementVector = new Vector2f();
            bool attacking = false;

            if (controller == null)
            {
                if (KInput.IsPressed(SFML.Window.Keyboard.Key.W))
                    --movementVector.Y;

                else if (KInput.IsPressed(SFML.Window.Keyboard.Key.S))
                    ++movementVector.Y;

                if (KInput.IsPressed(SFML.Window.Keyboard.Key.A))
                    --movementVector.X;

                else if (KInput.IsPressed(SFML.Window.Keyboard.Key.D))
                    ++movementVector.X;

                if (KInput.IsPressed(SFML.Window.Keyboard.Key.Space))
                    attacking = true;
            }
            else
            {
                const float THRESHOLD = 0.2f;
                controller.update();
                movementVector = controller.leftPad() / 100;
                movementVector.X = Math.Abs(movementVector.X) > THRESHOLD ? movementVector.X : 0.0f;
                movementVector.Y = Math.Abs(movementVector.Y) > THRESHOLD ? movementVector.Y : 0.0f;
                attacking = controller.isClicked(GInput.EButton.A);
            }

            input.Movement = movementVector;
            input.Attacking = attacking;

            return input;
        }

        public override void Update(GameTime gameTime, World.GameWorld world)
        {
            float speed = 400 * (float)gameTime.ElapsedTime.TotalSeconds;

            PlayerInput input = GetInput();
            Owner.Collider.Move(speed * input.Movement);

            if (KInput.IsPressed(SFML.Window.Keyboard.Key.Q))
                Owner.Collider.Rotate(-0.1f);

            else if (KInput.IsPressed(SFML.Window.Keyboard.Key.E))
                Owner.Collider.Rotate(0.1f);

            if (KInput.IsClicked(SFML.Window.Keyboard.Key.Space))
                Owner.Activator.Activate(gameTime, world);

            weapon.Update(gameTime, world);

            if (input.Attacking)
                weapon.Attack(world);

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
