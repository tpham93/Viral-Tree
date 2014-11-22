using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public class MathUtil
    {
        /// <summary> Pi * 2 = 6.283... Represents 360.0 degree in radian. </summary>
        public const float TWO_PI = (float)(Math.PI * 2);

        /// <summary> Pi = 3.1415... Represents 180.0 degree in radian.</summary>
        public const float PI = (float)(Math.PI);

        /// <summary> Pi / 2 = 1.5707... Represents 90.0 degree in radian.</summary>
        public const float PI_OVER_TWO = (float)(Math.PI * 0.5);

        /// <summary> Pi / 4 = 0.7853... Represents 45.0 degree in radian.</summary>
        public const float PI_OVER_FOUR = (float)(Math.PI * 0.25);

        /// <summary> Absolute float-values under this value are considered zero. </summary>
        public const float ZERO_TOLERANCE = 0.0001f;

        /// <summary>A global random generator. </summary>
        private static MyRandom random = new MyRandom();
        public static MyRandom Rand
        {
            get { return random; }
            private set { random = value; }
        }



        /// <summary>
        /// Clamps the given value val to [min, max].
        /// </summary>
        /// <typeparam name="T">The type of the value being clamped. It needs to implement IComparable.</typeparam>
        /// <param name="val">The value to be clamped.</param>
        /// <param name="min">The minimal value.</param>
        /// <param name="max">The maximal value.</param>
        /// <returns></returns>
        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            return val.CompareTo(min) < 0 ? min : val.CompareTo(max) > 0 ? max : val;
        }


        /// <summary>
        /// A cubic function that can be used for interpolation, weights the parameter.
        /// </summary>
        /// <param name="t">Percent of the smoothstep, should be between 0 and 1, but wont be checked.</param>
        /// <returns>A value between 0 and 1, but weighted.</returns>
        public static float Smoothstep(float t)
        {
            return t * t * (3.0f - 2.0f * t);
        }

        /// <summary>
        /// A cubic function that can be used for interpolation, with a certain minimal and maximal value.
        /// </summary>
        /// <param name="min">The minimal value of smoothstep.</param>
        /// <param name="max">The maximal value of smoothstep.</param>
        /// <param name="t">Value between 0 and 1, between min and max.</param>
        /// <returns>A value between min and max, with amount t being smoothstepped.</returns>
        public static float Smoothstep(float min, float max, float t)
        {
            return Smoothstep(t) * (max - min) + min;
        }

        /// <summary>
        /// A cubic function that can be used for interpolation, weights the parameter.
        /// </summary>
        /// <param name="t">Percent of the smoothstep, should be between 0 and 1, but wont be checked.</param>
        /// <returns>A value between 0 and 1, but weighted.</returns>
        public static double Smoothstep(double t)
        {
            return t * t * (3.0 - 2.0 * t);
        }

        public static double Smoothstep(double min, double max, double t)
        {
            return Smoothstep(t) * (max - min) + min;
        }


        public static bool NearEqual(float a, float b)
        {
            return Math.Abs(a - b) < ZERO_TOLERANCE;
        }

        public static bool NearEqual(float a, float b, float maxDiff)
        {
            return Math.Abs(a - b) < maxDiff;
        }


        /// <summary>
        /// Swaps the content of the given variables.
        /// </summary>
        /// <typeparam name="T">The type of the values being swapped.</typeparam>
        /// <param name="a">First variable.</param>
        /// <param name="b">Second variable.</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            T tmp = a;
            a = b;
            b = tmp;
        }

        /// <summary>
        /// Computes the degree value of the given radian value.
        /// </summary>
        /// <param name="rad">The value in radian-</param>
        /// <returns>The value in degree.</returns>
        public static float ToDegree(float rad)
        {
            return (rad / PI) * 180.0f;
        }

        /// <summary>
        /// Computes the radian value of the given degree value.
        /// </summary>
        /// <param name="degree">The degree value.</param>
        /// <returns>The radian value.</returns>
        public static float ToRad(float degree)
        {
            return (degree / 180.0f) * PI;
        }


        public static bool IsEqual(Vector2i a, Vector2i b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool IsEqual(Vector2u a, Vector2u b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        //TODO: maybe put to Vec2f
        public Vector2f EvaluateDeCasteljau(Vector2f[] points, float t)
        {
            if (points.Length == 1)
                return points[0];

            else
            {
                Vector2f[] subPoints = new Vector2f[points.Length - 1];

                for (int i = 0; i < subPoints.Length; i++)
                    subPoints[i] = Vec2f.Lerp(points[i], points[(i + 1) == points.Length ? 0 : (i + 1)], t);

                return EvaluateDeCasteljau(subPoints, t);
            }
        }

        public static T[] ToArray<T>(List<T> objs)
        {
            T[] result = new T[objs.Count];

            for (int i = 0; i < objs.Count; i++)
                result[i] = objs[i];

            return result;
        }

    }
}
