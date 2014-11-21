using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public struct Matrix2D
    {
        /// <summary>
        /// Matrix that does nothing on transformation -> [1 0 \n 0 1]
        /// </summary>
        public static Matrix2D Identity
        {
            get { return new Matrix2D(1.0f, 0.0f, 0.0f, 1.0f); }
        }

        float[] a;

        /// <summary>
        /// Construcs a Matrix2D with the given floats. Looks like:
        /// {a, b} \n {c, d}
        /// </summary>
        /// <param name="a">a00 entry of matrix.</param>
        /// <param name="b">a10 entry of matrix.</param>
        /// <param name="c">a01 entry of matrix.</param>
        /// <param name="d">a11 entry of matrix.</param>
        private Matrix2D(float a, float b, float c, float d)
        {
            this.a = new float[4] { a, b, c, d };
        }

        /// <summary>
        /// Converts the current Matrix2D to a rotation matrix with a rotation by rad radians.
        /// </summary>
        /// <param name="rad"></param>
        public void convertToRotation(float rad)
        {
            float sin = (float)Math.Sin(rad);
            float cos = (float)Math.Cos(rad);

            this.a[0] = cos;
            this.a[1] = -sin;
            this.a[2] = sin;
            this.a[3] = cos;
        }

        /// <summary>
        /// Converts the current Matrix2D to a scaling matrix with dX and dY scaling in percentage.
        /// </summary>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        public void convertToScale(float dX, float dY)
        {
            this.a[0] = dX;
            this.a[1] = 0;
            this.a[2] = 0;
            this.a[3] = dY;
        }

        private void convertToMirror(Vector2f mirrorLine)
        {
            float lenSq = Vec2f.LengthSq(mirrorLine);

            a[0] = (mirrorLine.X * mirrorLine.X - mirrorLine.Y * mirrorLine.Y) / lenSq;
            a[1] = (2.0f * mirrorLine.X * mirrorLine.Y) / lenSq;
            a[2] = a[1];
            a[3] = (mirrorLine.Y * mirrorLine.Y - mirrorLine.X * mirrorLine.X) / lenSq;
        }

        /// <summary>
        /// Multiplicates the given Matrix2D with a Vector2f (applying the transformation to the vector).
        /// </summary>
        /// <param name="v">The vector to be transformed.</param>
        /// <returns>The transformed vector.</returns>
        public Vector2f mult(Vector2f v)
        {
            return new Vector2f(this.a[0] * v.X + this.a[1] * v.Y, this.a[2] * v.X + this.a[3] * v.Y);
        }

        /// <summary>
        /// Creates a Matrix2D that can scale vectors.
        /// </summary>
        /// <param name="dX"></param>
        /// <param name="dY"></param>
        /// <returns></returns>
        public static Matrix2D CreateScale(float dX, float dY)
        {
            return new Matrix2D(dX, 0, 0, dY);
        }

        /// <summary>
        /// Creates a Matrix2D that can rotate vectors.
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Matrix2D CreateRotation(float rad)
        {
            float sin = (float)Math.Sin(rad);
            float cos = (float)Math.Cos(rad);

            return new Matrix2D(cos, -sin, sin, cos);
        }

        public static Matrix2D CreateMirror(Vector2f mirrorLine)
        {
            float lenSq = Vec2f.LengthSq(mirrorLine);
            float tmp = (2.0f * mirrorLine.X * mirrorLine.Y) / lenSq;


            return new Matrix2D((mirrorLine.X * mirrorLine.X - mirrorLine.Y * mirrorLine.Y) / lenSq,
                                tmp,
                                tmp,
                                (mirrorLine.Y * mirrorLine.Y - mirrorLine.X * mirrorLine.X) / lenSq);
        }

    }
}
