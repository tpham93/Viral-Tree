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
        public enum EStick { LUp, LDown, LLeft, LRight,LMiddle, RUp, RDown, RLeft, RRight,RMiddle, StickCount };

        private bool[] currentButtons;
        private bool[] prevButtons;

        public EStick currentLeftStick;
        public EStick prevLeftStick;

        public EStick currentRightStick;
        public EStick prevRightStick;

        uint index;
        
        public static List<uint> getConnectedGamepads()
        {
            List<uint> connectedGamepads = new List<uint>();
            for(uint i = 0; i < Joystick.Count; ++i)
            {
                if(Joystick.IsConnected(i))
                {
                    connectedGamepads.Add(i);
                }
            }
            return connectedGamepads;
        }

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
                currentButtons[i] = Joystick.IsButtonPressed(index, i);
            }
            prevLeftStick = currentLeftStick;
            prevRightStick = currentRightStick;

            currentLeftStick = updateLeftStick();
            currentRightStick = updateRightStick();

            
        }

        public bool isClicked(EButton button)
        {
            return currentButtons[(int)button] && !prevButtons[(int)button];
        }

        public bool isClicked(EStick stick)
        {
            return ((prevLeftStick == EStick.LLeft && currentLeftStick == EStick.LMiddle) || stick == currentLeftStick);
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

        public EStick updateLeftStick()
        {
            if (rightPad().X < -90)
            {
                return EStick.RLeft;
            }
            else if (rightPad().X > 90)
            {
                return EStick.RRight;
            }
            else if (rightPad().Y > 90)
            {
                return EStick.RUp;
            }
            else if (rightPad().Y < -90)
            {
                return EStick.RDown;
            }
            else
                return EStick.RMiddle;
        }

        public EStick updateRightStick()
        {
            if (rightPad().X < -90)
            {
                return EStick.LLeft;
            }
            else if (leftPad().X > 90)
            {
                return EStick.LRight;
            }
            else if (leftPad().Y > 90)
            {
                return EStick.LUp;
            }
            else if (leftPad().Y < -90)
            {
                return EStick.LDown;
            }
            else
                return EStick.LMiddle;
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
