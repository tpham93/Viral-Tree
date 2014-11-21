using SFML.Graphics;
using SFML.Window;
using System;

namespace ViralTree
{

    /// <summary>
    /// Represents mouse. Just call update() every tick once (at the beginning).
    /// </summary>
    public static class MInput
    {
        private static bool[] oldMouse;
        private static bool[] currentMouse;

        private static Vector2i oldMousePos;
        private static Vector2i currentMousePos;

        private static Game game;

        /// <summary>
        /// Creates the MInput.
        /// </summary>
        /// <param name="game"></param>
        public static void Init(Game game)
        {
            MInput.game = game;

            oldMouse = new bool[(int)Mouse.Button.ButtonCount];
            currentMouse = new bool[oldMouse.Length];
        }

        /// <summary>
        /// Updates the internal state of MInput, such as position, button pressed etc.
        /// </summary>
        public static void Update()
        {
            oldMousePos = currentMousePos;
            currentMousePos = Mouse.GetPosition(game.window);

            for (int i = 0; i < oldMouse.Length; i++)
            {
                oldMouse[i] = currentMouse[i];
                currentMouse[i] = Mouse.IsButtonPressed((Mouse.Button)i);
            }
        }

        /// <summary>
        /// Returns the mouse position on this frame, relative to the window.
        /// </summary>
        /// <returns></returns>
        public static Vector2f GetCurPos()
        {
            return new Vector2f(currentMousePos.X, currentMousePos.Y);
        }

        /// <summary>
        /// Returns the mouse position on last frame, relative to the window.
        /// </summary>
        /// <returns></returns>
        public static Vector2f GetOldPos()
        {
            return new Vector2f(oldMousePos.X, oldMousePos.Y);
        }

        public static Vector2f GetMousePos(Vector2f offset)
        {
            return new Vector2f(currentMousePos.X + offset.X, currentMousePos.Y + offset.Y);
        }

        public static Vector2f GetOldMousePos(Vector2f offset)
        {
            return new Vector2f(oldMousePos.X + offset.X, oldMousePos.Y + offset.Y);
        }

        /// <summary>
        /// Returns the direction the mouse is moved since the last frame.
        /// </summary>
        /// <returns></returns>
        public static Vector2f GetMouseDir()
        {
            return GetCurPos() - GetOldPos();
        }


        /// <summary>
        /// Checks if the left mouse button was clicked (presser right now and not pressed last tick).
        /// </summary>
        /// <returns>True if the left mouse button was clicked.</returns>
        public static bool LeftClicked()
        {
            return !oldMouse[(int)Mouse.Button.Left] && currentMouse[(int)Mouse.Button.Left];
        }

        /// <summary>
        /// Checks if the left mouse button is pressed.
        /// </summary>
        /// <returns>True if the left button is pressed right now.</returns>
        public static bool LeftPressed()
        {
            return currentMouse[(int)Mouse.Button.Left];
        }

        /// <summary>
        /// Checks if the left mouse button was released (not pressed right now and pressed last tick).
        /// </summary>
        /// <returns>True if the left mouse button is released, false otherwise.</returns>
        public static bool LeftReleased()
        {
            return oldMouse[(int)Mouse.Button.Left] && !currentMouse[(int)Mouse.Button.Left];
        }


        /// <summary>
        /// Checks if the right mouse button was clicked this tick (pressed right now and not pressed last tick).
        /// </summary>
        /// <returns>True if the right mouse button is clicked, false otherwise.</returns>
        public static bool RightClicked()
        {
            return !oldMouse[(int)Mouse.Button.Right] && currentMouse[(int)Mouse.Button.Right];
        }

        /// <summary>
        /// Checks if the right mouse button is pressed this tick (pressed right now).
        /// </summary>
        /// <returns>True if the right mouse button is pressed, false otherwise.</returns>
        public static bool RightPressed()
        {
            return currentMouse[(int)Mouse.Button.Right];
        }

        /// <summary>
        /// Checks if the right mouse button was released this tick (not pressed right now and pressed last tick).
        /// </summary>
        /// <returns>True if the right mouse button is released, false otherwise.</returns>
        public static bool RightReleased()
        {
            return oldMouse[(int)Mouse.Button.Right] && !currentMouse[(int)Mouse.Button.Right];
        }

        /// <summary>
        /// Checks if the middle mouse button was clicked this tick (pressed right now and not pressed last tick).
        /// </summary>
        /// <returns>True if the middle mouse button is clicked, false otherwise.</returns>
        public static bool MidClicked()
        {
            return !oldMouse[(int)Mouse.Button.Middle] && currentMouse[(int)Mouse.Button.Middle];
        }

        /// <summary>
        /// Checks if the middle mouse button was was pressed this tick (pressed right now).
        /// </summary>
        /// <returns>True if the middle mouse button is pressed, false otherwise.</returns>
        public static bool MidPressed()
        {
            return currentMouse[(int)Mouse.Button.Middle];
        }

        /// <summary>
        /// Checks if the middle mouse button was released this tick (not pressed right now and pressed last tick).
        /// </summary>
        /// <returns>True if the middle mouse button was released this tick, false otherwise.</returns>
        public static bool MidReleased()
        {
            return oldMouse[(int)Mouse.Button.Middle] && !currentMouse[(int)Mouse.Button.Middle];
        }

        /// <summary>
        /// Checks if the mouse wheel is scrolled up.
        /// </summary>
        /// <returns>True if the mouse wheel was scrolled up this tick.</returns>
        public static bool MouseWheelUp()
        {
            return game.mouseWheel() == -1;
        }

        /// <summary>
        /// Checks if the mouse wheel was scrolled down.
        /// </summary>
        /// <returns>True if the mouse wheel was scrolled down this tick.</returns>
        public static bool MouseWheelDown()
        {
            return game.mouseWheel() == 1;
        }
    }

}