using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public static class Vec2f
    {
        /// <summary> Vector2f with both components set to zero. </summary>
        public static Vector2f Zero { get { return new Vector2f(0, 0); } }

        /// <summary> Vector2f with both x = 1 and y = 0. </summary>
        public static Vector2f UnitX { get { return new Vector2f(1, 0); } }

        /// <summary> Vector2f with both x = 0 and y = 1. </summary>
        public static Vector2f UnitY { get { return new Vector2f(0, 1); } }

        /// <summary> Vector2f with both components set to one. </summary>
        public static Vector2f One { get { return new Vector2f(1, 1); } }

        /// <summary> Returns a random direction with normalized length. </summary>
        public static Vector2f RandomDir
        {
            get
            {
                double rand = Math.PI * 2.0 * MathUtil.Rand.NextDouble();
                return new Vector2f((float)Math.Cos(rand),
                                    (float)Math.Sin(rand));
            }
        }

        /// <summary>
        /// Determined if two given Vector2f are almost equal with the maximal difference of <see cref="MathUtil.ZERO_TOLERANCE"/>
        /// </summary>
        /// <param name="a">First Vector2f.</param>
        /// <param name="b">Second Vector2f.</param>
        /// <param name="maxDiff">The maximal difference between the given Vector2f.</param>
        /// <returns>True, if the absolute value of x and y component is less than maxDiff. False otherwise.</returns>
        public static bool NearEqual(Vector2f a, Vector2f b)
        {
            return MathUtil.NearEqual(a.X, b.X) && MathUtil.NearEqual(a.Y, b.Y);
        }

        /// <summary>
        /// Determined if two given Vector2f are almost equal, with a given maximal difference.
        /// </summary>
        /// <param name="a">First Vector2f.</param>
        /// <param name="b">Second Vector2f.</param>
        /// <param name="maxDiff">The maximal difference between the given Vector2f.</param>
        /// <returns>True, if the absolute value of x and y component is less than maxDiff. False otherwise.</returns>
        public static bool NearEqual(Vector2f a, Vector2f b, float maxDiff)
        {
            return MathUtil.NearEqual(a.X, b.X, maxDiff) && MathUtil.NearEqual(a.Y, b.Y, maxDiff);
        }


        /// <summary>
        /// Computed the dot product (scalar product, the projection onto another Vector2f) between two given Vector2f. 
        /// </summary>
        /// <param name="left">First Vector2f who will be projected.</param>
        /// <param name="right">Second Vector2f to be projected on</param>
        /// <returns>The result of the dot product.</returns>
        public static float Dot(Vector2f left, Vector2f right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        /// <summary>
        /// Computes the length of a given Vector2f.
        /// </summary>
        /// <param name="left">The Vector2f whose length will be computed.</param>
        /// <returns>The length of the given Vector2f.</returns>
        public static float Length(Vector2f left)
        {
            return (float)Math.Sqrt(left.X * left.X + left.Y * left.Y);
        }

        /// <summary>
        /// Computes the squared length of a given Vector2f.
        /// </summary>
        /// <param name="left">The Vector2f whose squared length will be computed.</param>
        /// <returns>The squared length of the given Vector2f.</returns>
        public static float LengthSq(Vector2f left)
        {
            return left.X * left.X + left.Y * left.Y;
        }

        /// <summary>
        /// Computes the euclidian distance between two given Vector2f.
        /// </summary>
        /// <param name="left">First Vector2f.</param>
        /// <param name="right">Second Vector2f.</param>
        /// <returns>The distance.</returns>
        public static float EuclidianDistance(Vector2f left, Vector2f right)
        {
            return Length(right - left);
        }

        /// <summary>
        /// Computes the squared euclidian distance between two given Vector2f.
        /// </summary>
        /// <param name="left">First Vector2f.</param>
        /// <param name="right">Second Vector2f</param>
        /// <returns>The squared distance.</returns>
        public static float EuclidianDistanceSq(Vector2f left, Vector2f right)
        {
            return LengthSq(right - left);
        }

        /// <summary>
        /// Computes the distance in 4 neighbourhood.
        /// </summary>
        public static float CityblockDistance(Vector2f left, Vector2f right)
        {
            return Math.Abs(right.X - left.X) + Math.Abs(right.Y - left.Y);
        }

        /// <summary>
        /// Computes the distance in 8 neighbourhood.
        /// </summary>
        public static float ChessDistance(Vector2f left, Vector2f right)
        {
            return Math.Max(Math.Abs(right.X - left.X), Math.Abs(right.Y - left.Y));
        }

        /// <summary>
        /// Computes the angle between the given direction and the x axis.
        /// </summary>
        /// <param name="direction">The Vector2f whose angle is to be computed.</param>
        /// <returns>The value that represents the given direction.</returns>
        public static float RotationFrom(Vector2f direction)
        {
            return (float)Math.Atan2(direction.Y, direction.X);
        }

        /// <summary>
        /// Computes the direction from (0, 0) represented by a given radian value.
        /// </summary>
        /// <param name="rad">The value that represend the Vector2f: 0 -> (1, 0), PI -> (-1, 0) etc.</param>
        /// <returns>The direction whose angle is rad on the x axis.</returns>
        public static Vector2f DirectionFrom(float rad)
        {
            return new Vector2f((float)Math.Cos(rad), (float)Math.Sin(rad));
        }

        /// <summary>
        /// Rotates a given Vector2f around a certain rotation center with a given amount in radian.
        /// </summary>
        /// <param name="v">The Vector2f being rotated.</param>
        /// <param name="rotationCenter">The center of rotation.</param>
        /// <param name="rad">The amount of rotation in radian.</param>
        /// <returns>Rotated input Vector2f v.</returns>
        public static Vector2f RotateTransform(Vector2f v, Vector2f rotationCenter, float rad)
        {
            return Matrix2D.CreateRotation(rad).mult(v - rotationCenter) + rotationCenter;
        }

        /// <summary>
        /// Scales a given Vector2f with a certain amount.
        /// </summary>
        /// <param name="v">The Vector2f being scaled. CARE: will be scaled from (0, 0).</param>
        /// <param name="scale">The amount of scaling in percent.</param>
        /// <returns>Scaled input Vector2f v.</returns>
        public static Vector2f ScaleTransform(Vector2f v, Vector2f scale)
        {
            return Matrix2D.CreateScale(scale.X, scale.Y).mult(v);
        }


        //TODO: doc, finish
        public static Vector2f MirrorTransform(Vector2f v, Vector2f mirrorLine)
        {
            return Matrix2D.CreateMirror(mirrorLine).mult(v);
        }

        /// <summary>
        /// Negates the x component of a given Vector2f.
        /// </summary>
        /// <param name="left">The Vector2f whose x component is going to be negated</param>
        /// <returns>Input Vector2f with negated x component.</returns>
        public static Vector2f NegatedX(Vector2f left)
        {
            return new Vector2f(-left.X, left.Y);
        }

        /// <summary>
        /// Negates the y component of a given Vector2f.
        /// </summary>
        /// <param name="left">The Vector2f whose y component is going to be negated</param>
        /// <returns>Input Vector2f with negated y component.</returns>
        public static Vector2f NegatedY(Vector2f left)
        {
            return new Vector2f(left.X, -left.Y);
        }

        /// <summary>
        /// Flips the given Vector2f -> swaps the x and y component.
        /// </summary>
        /// <param name="vec">The Vector2f intended being flipped</param>
        /// <returns>The Vector2f whose x and y components are swapped.</returns>
        public static Vector2f Flipped(Vector2f vec)
        {
            return new Vector2f(vec.Y, vec.X);
        }

        /// <summary>
        /// Normalized the given Vector2f (makes its length to 1). If the given vector has length 0, nothing happens.
        /// </summary>
        /// <param name="vec">Vector2f being normalized.</param>
        /// <returns>Vector2f with length 1.</returns>
        public static Vector2f Normalized(Vector2f vec)
        {
            float l = Length(vec);

            if (l == 0.0f)
                return vec;

            else
                return vec / l;
        }

        public static Vector2f Normalized(Vector2f vec, float length)
        {
            if (length == 0.0f)
                return vec;

            else
                return vec / length;
        }

        /// <summary>
        /// Linear interpolation between two Vector2f with the given amount t.
        /// </summary>
        /// <param name="left">The starting Vector2f.</param>
        /// <param name="other">The ending Vector2f.</param>
        /// <param name="t">Amount of interpolation, should from interval [0, 1].</param>
        /// <returns>Linear interpolation of left and right with amount t.</returns>
        public static Vector2f Lerp(Vector2f left, Vector2f other, float t)
        {
            return (1.0f - t) * left + t * other;
        }

        /// <summary>
        /// Reflects the given Vector2f onto a given normal.
        /// </summary>
        /// <param name="direction">The direction being reflected.</param>
        /// <param name="normal">The normal to be reflected on.</param>
        /// <returns>Reflected Vector2f.</returns>
        public static Vector2f Reflect(Vector2f direction, Vector2f normal)
        {
            return direction - 2.0f * Dot(direction, normal) * normal;
        }

        /// <summary>
        /// Computes the left normal of a given Vector2f and normalizes it.
        /// </summary>
        /// <param name="direction">The Vector2f whose left normal being computed.</param>
        /// <returns>Left normal of the given direction.</returns>
        public static Vector2f LeftNormal(Vector2f direction)
        {
            return Vec2f.Normalized(new Vector2f(direction.Y, -direction.X));
        }



        /// <summary>
        /// Computes the right normal of a given Vector2f and normalizes it.
        /// </summary>
        /// <param name="direction">The Vector2f whose right normal being computed.</param>
        /// <returns>Right normal of the given direction.</returns>
        public static Vector2f RightNormal(Vector2f direction)
        {
            return Vec2f.Normalized(new Vector2f(-direction.Y, direction.X));
        }

        /// <summary>
        /// Computes the left perpendicular of the given Vector2f with the same length.
        /// </summary>
        /// <param name="dir">The Vector2f whose left perpendicular will be computed.</param>
        /// <returns>Left perpendicular, with same length</returns>
        public static Vector2f LeftPerpendicular(Vector2f dir)
        {
            return new Vector2f(dir.Y, -dir.X);
        }

        /// <summary>
        /// Computes the right perpendicular of the given Vector2f with the same length.
        /// </summary>
        /// <param name="dir">The Vector2f whose right perpendicular will be computed.</param>
        /// <returns>Right perpendicular, with same length</returns>
        public static Vector2f RightPerpendicular(Vector2f dir)
        {
            return new Vector2f(-dir.Y, dir.X);
        }



        //Added from internet for custom physics engine:

        public static float CrossProduct(Vector2f a, Vector2f b)
        {
            return a.X * b.Y - a.Y * b.X;
        }

        public static Vector2f CrossProduct(Vector2f a, float s)
        {
            return new Vector2f(s * a.Y, -s * a.X);
        }

        public static Vector2f CrossProduct(float s, Vector2f a)
        {
            return new Vector2f(-s * a.Y, s * a.X);
        }

        public static Vector2i LeftPerpendicular(Vector2i dir)
        {
            return new Vector2i(dir.Y, -dir.X);
        }

        /// <summary>
        /// Computes the right perpendicular of the given Vector2f with the same length.
        /// </summary>
        /// <param name="dir">The Vector2f whose right perpendicular will be computed.</param>
        /// <returns>Right perpendicular, with same length</returns>
        public static Vector2i RightPerpendicular(Vector2i dir)
        {
            return new Vector2i(-dir.Y, dir.X);
        }
    }
}
