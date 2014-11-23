using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ViralTree.World;

namespace ViralTree.Components
{
    public sealed class PlayerThinker : AThinker
    {
        private GInput controller;
        private AWeapon weapon;
        private AWeapon specialWeapon;

        public PlayerThinker(AWeapon weapon, AWeapon specialWeapon, GInput controller = null)
        {
            this.controller = controller;

            this.weapon = weapon;
            this.specialWeapon = specialWeapon;

        //    weapon = new ShooterWeapon(30, TimeSpan.FromMilliseconds(GameplayConstants.PLAYER_SHOOTER_FREQ), new CircleCollider(16), float.PositiveInfinity, GameplayConstants.PLAYER_SHOOTER_DAMAGE, GameplayConstants.PLAYER_SHOOTER_SPEED);
         //   specialWeapon = new ScoutSpecial(TimeSpan.FromMilliseconds(GameplayConstants.SCOUT_SPECIAL_FREQ), TimeSpan.FromMilliseconds(GameplayConstants.SCOUT_SPECIAL_DURATION), TimeSpan.FromMilliseconds(GameplayConstants.SCOUT_DECREASED_FREQ), weapon);
        }

        public override void Initialize()
        {
            base.Initialize();

            weapon.Owner = this.Owner;
            specialWeapon.Owner = this.Owner;
        }

        private PlayerInput GetInput(GameWorld world)
        {
            PlayerInput input = new PlayerInput();
            Vector2f movementVector = new Vector2f();
            Vector2f direction = new Vector2f();
            bool attacking = false;
            bool special = false;

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

                attacking = MInput.LeftPressed();
                special = MInput.RightClicked();

                Vector2f screenPos = Owner.Collider.Position - world.Cam.Position;
                screenPos.X *= Settings.WindowSize.X / world.Cam.currentView.Size.X;
                screenPos.Y *= Settings.WindowSize.Y / world.Cam.currentView.Size.Y;

              //  Console.WriteLine(screenPos);

                direction = MInput.GetCurPos() - screenPos - new Vector2f(Settings.WindowSize.X / 2.0f, Settings.WindowSize.Y / 2.0f);
            }
            else
            {
                const float THRESHOLD = 0.4f;
                controller.update();
                Vector2f controllerMovement = controller.leftPad() / 100;

                if (Math.Abs(controllerMovement.X) + Math.Abs(controllerMovement.Y) > THRESHOLD)
                {
                    movementVector.X = controllerMovement.X;
                    movementVector.Y = controllerMovement.Y;
                }

                Vector2f controllerDirection = controller.rightPad() / 100;

                if (Math.Abs(controllerDirection.X) + Math.Abs(controllerDirection.Y) > THRESHOLD)
                {
                    direction.X = controllerDirection.X;
                    direction.Y = controllerDirection.Y;
                }

                attacking = controller.isPressed(GInput.EButton.RB);
                special = controller.isPressed(GInput.EButton.LB);
            }

            input.Movement = movementVector;
            input.Attacking = attacking;
            input.Direction = direction;
            input.Special = special;

            return input;
        }

        public override void Update(GameTime gameTime, GameWorld world)
        {
            float speed = GameplayConstants.PLAYER_MAX_SPEED * (float)gameTime.ElapsedTime.TotalSeconds;

            PlayerInput input = GetInput(world);
            Owner.Collider.Move(speed * input.Movement);
            Owner.Collider.Direction += input.Direction;

            /*
            if (KInput.IsPressed(SFML.Window.Keyboard.Key.Q))
                Owner.Collider.Rotate(-0.1f);

            else if (KInput.IsPressed(SFML.Window.Keyboard.Key.E))
                Owner.Collider.Rotate(0.1f)*/

            weapon.Update(gameTime, world);
            specialWeapon.Update(gameTime, world);




            if (input.Attacking)
                weapon.Attack(world, gameTime);

            if (input.Special)
                specialWeapon.Attack(world, gameTime);

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
