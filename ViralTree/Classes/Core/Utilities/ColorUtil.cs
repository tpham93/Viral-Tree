using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public static class ColorUtil
    {
        public static Color CornflowerBlue { get { return new Color(100, 149, 237); } }
        public static Color Orange { get { return new Color(255, 140, 0); } }


        public static Color Add(Color left, Color right)
        {
            return new Color(Math.Min((byte)(left.R + right.R), byte.MaxValue),
                                Math.Min((byte)(left.G + right.G), byte.MaxValue),
                                Math.Min((byte)(left.B + right.B), byte.MaxValue),
                                Math.Min((byte)(left.A + right.A), byte.MaxValue));
        }

        public static Color Sub(Color left, Color right)
        {
            return new Color(Math.Max((byte)(left.R - right.R), byte.MinValue),
                                Math.Max((byte)(left.G - right.G), byte.MinValue),
                                Math.Max((byte)(left.B - right.B), byte.MinValue),
                                Math.Max((byte)(left.A - right.A), byte.MinValue));
        }

        public static Color Mult(Color left, float right)
        {
            return new Color(Math.Max(Math.Min((byte)(left.R * right), byte.MaxValue), byte.MinValue),
                                Math.Max(Math.Min((byte)(left.G * right), byte.MaxValue), byte.MinValue),
                                Math.Max(Math.Min((byte)(left.B * right), byte.MaxValue), byte.MinValue),
                                Math.Max(Math.Min((byte)(left.A * right), byte.MaxValue), byte.MinValue));
        }

        public static Color Mult(float left, Color right)
        {
            return Mult(right, left);
        }

        public static Color Lerp(Color c1, Color c2, float value)
        {
            return Add(Mult(c1, 1.0f - value), Mult(c2, value));
        }

        public static Color Negate(Color c1)
        {
            return Sub(Color.White, c1);
        }

    }
}
