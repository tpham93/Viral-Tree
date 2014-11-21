using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public class GInput
    {
        public enum EButton { A, B, X, Y, LB, RB, Back, Start, L3, R3, ButtonCount };

        private bool[] currentButtons;
        private bool[] prevButtons;

        uint index;

        public GInput(uint index)
        {
            this.index = index;

            currentButtons = new bool[(int)EButton.ButtonCount];
            prevButtons = new bool[currentButtons.Length];
        }

        /// <summary>
        /// Requires SFML.Window.Joystick.Update() before this call.
        /// </summary>
        public void update()
        {
            for (uint i = 0; i < currentButtons.Length; i++)
            {
                prevButtons[i] = currentButtons[i];
                currentButtons[i] = Joystick.IsButtonPressed(0, i);
            }
        }

        public bool isClicked(EButton button)
        {
            return currentButtons[(int)button] && !prevButtons[(int)button];
        }

        public bool isPressed(EButton button)
        {
            return currentButtons[(int)button];
        }

        public bool isReleased(EButton button)
        {
            return !currentButtons[(int)button] && prevButtons[(int)button];
        }

        private Vector2f getAxis(Joystick.Axis a, Joystick.Axis b)
        {
            return new Vector2f(Joystick.GetAxisPosition(index, a), Joystick.GetAxisPosition(index, b));
        }

        public Vector2f dPad()
        {
            return Vec2f.NegatedY(getAxis(Joystick.Axis.PovY, Joystick.Axis.PovX));
        }

        public Vector2f rightPad()
        {
            return getAxis(Joystick.Axis.U, Joystick.Axis.R);
        }

        public Vector2f leftPad()
        {
            return getAxis(Joystick.Axis.X, Joystick.Axis.Y);
        }

        /// <summary>
        /// Negative values for left bumper, positive values for right bumper.
        /// </summary>
        /// <returns></returns>
        public float bumber()
        {
            return -Joystick.GetAxisPosition(index, Joystick.Axis.Z);
        }


    }
}
