﻿using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public static class PolygonFactory
    {

        public static ConvexCollider getRegularPolygon(int numEdges, float radius)
        {
            Vector2f[] vertices = new Vector2f[numEdges];

            float rad = MathUtil.TWO_PI / numEdges;

            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = Vec2f.DirectionFrom(-rad * i) * radius;

            return new ConvexCollider(vertices, numEdges % 2 == 0);
        }

        //TODO: optimisation / maybe find better subdivision of convexPolygons
        public static ConcaveCollider getRegularStar(int numEdges, float radius)
        {
            List<ConvexCollider> list = new List<ConvexCollider>();

            float rad = MathUtil.TWO_PI / numEdges;

          
            for (int i = 0; i < numEdges; i++)
            {
                Vector2f current = Vec2f.DirectionFrom(-i * rad) * radius;

                Vector2f subNext = (current + Vec2f.DirectionFrom(-((i + 1) % numEdges) * rad) * radius) / 3.0f;

                Vector2f subPrev = (current + Vec2f.DirectionFrom(-((i + numEdges - 1) % numEdges) * rad) * radius) / 3.0f;

                Vector2f[] currentVertices = { subNext, Vec2f.Zero, subPrev, current };

                list.Add(new ConvexCollider(currentVertices, false));
            }

            return new ConcaveCollider(list, Vec2f.Zero, 4 * numEdges);
        }

        public static ConvexCollider GetElipse(int numEdges, float radiusX, float radiusY)
        {
            Vector2f[] vertices = new Vector2f[numEdges];

            double delta = (Math.PI * 2.0) / numEdges;

            for (int i = 0; i < vertices.Length; i++)
            {
                float xVal = (float)Math.Cos(i * delta) * radiusX;
                float yVal = (float)Math.Sin(i * delta) * radiusY;

                Vector2f current = new Vector2f(xVal, yVal);

                vertices[i] = current;
            }


            //maybe remove redundantNormals:
            return new ConvexCollider(vertices, numEdges % 2 == 0);
        }

        public static ConvexCollider GetHalfElipse(int numEdges, float radX, float radY)
        {
            Vector2f[] vertices = new Vector2f[numEdges];

            double delta = Math.PI / numEdges;

            for (int i = 0; i < vertices.Length; i++)
            {
                float xVal = (float)Math.Cos(i * delta - MathUtil.PI_OVER_TWO) * radX;
                float yVal = (float)Math.Sin(i * delta - MathUtil.PI_OVER_TWO) * radY;

                Vector2f current = new Vector2f(xVal, yVal);

                vertices[i] = current;
            }


            //maybe remove redundantNormals:
            return new ConvexCollider(vertices, numEdges % 2 == 0);
        }




    }
}
