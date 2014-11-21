using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public class ConcaveCollider : ACollider
    {

        private List<ConvexCollider> subPolys;

        private bool redundantPolys;

        public int VertexCount
        {
            get;
            private set;
        }

        public ConcaveCollider(List<ConvexCollider> info, Vector2f center, int vertexCount)
            : base(ECollidable.Concave)
        {
            subPolys = info;

            this.position = center;
            VertexCount = vertexCount;

            computerRadius();
            CalculateBoundingRectangle();
        }



        public override IntersectionData Intersects(ACollider other)
        {
            if (other.CollidableType == ECollidable.Circle)
                return SatCircle(other as CircleCollider, false);

            else if (other.CollidableType == ECollidable.Convex)
                return SatConvex(other as ConvexCollider, false);


            else if (other.CollidableType == ECollidable.Concave)
                return SatConcave(other as ConcaveCollider);

            else
                return new IntersectionData();
        }

        public IntersectionData SatCircle(CircleCollider other, bool swapDir)
        {
            if (this.IntersectsBounding(other))
            {
                IntersectionData totalData = new IntersectionData();
                totalData.peneValues = new List<float>();
                totalData.separationDirections = new List<Vector2f>();

                for (int i = 0; i < subPolys.Count; i++)
                {
                    //test if the given SubPolygon intersects with the other given Polygon:
                    IntersectionData data = subPolys[i].SatCircle(other);

                    //if they intersect, store the data inside totalData.
                    if (data.Intersects)
                    {
                        //add the new values for the given intersection:
                        totalData.peneValues.Add(data.peneValues[0]);

                        if (swapDir)
                            totalData.separationDirections.Add(-data.separationDirections[0]);
                        else
                            totalData.separationDirections.Add(data.separationDirections[0]);
                    }
                }

                return totalData;
            }
            else
                return new IntersectionData();
        }

        public IntersectionData SatConcave(ConcaveCollider other)
        {
            if (this.IntersectsBounding(other))
            {

                IntersectionData totalData = new IntersectionData();
                totalData.peneValues = new List<float>();
                totalData.separationDirections = new List<Vector2f>();


                for (int i = 0; i < this.subPolys.Count; i++)
                {
                    IntersectionData tmpData = this.SatConvex(other.subPolys[i], false);

                    if (tmpData.Intersects)
                    {
                        for (int j = 0; j < tmpData.peneValues.Count; j++)
                        {
                            totalData.peneValues.Add(tmpData.peneValues[j]);
                            totalData.separationDirections.Add(tmpData.separationDirections[j]);
                        }
                    }
                }

                return totalData;
            }
            else
                return new IntersectionData();

        }

        //TODO: maybe add maximum intersections (e.g. 2)
        public IntersectionData SatConvex(ConvexCollider other, bool swapDir)
        {
            if (this.IntersectsBounding(other))
            {
                //create PolyColData where to store the intersection information.
                IntersectionData totalData = new IntersectionData();
                totalData.peneValues = new List<float>();
                totalData.separationDirections = new List<Vector2f>();

                //go throw all ConvexPoly's this ConcavePoly is made of:
                for (int i = 0; i < subPolys.Count; i++)
                {
                    //test if the given SubPolygon intersects with the other given Polygon:
                    IntersectionData data = subPolys[i].SatConvex(other);

                    //if they intersect, store the data inside totalData.
                    if (data.Intersects)
                    {
                        //add the new values for the given intersection:
                        totalData.peneValues.Add(data.peneValues[0]);

                        if (swapDir)
                            totalData.separationDirections.Add(-data.separationDirections[0]);
                        else
                            totalData.separationDirections.Add(data.separationDirections[0]);
                    }

                }
                return totalData;

            }
            else
                return new IntersectionData();
        }


        public override void Rotate(float deltaRad)
        {
            for (int i = 0; i < subPolys.Count; i++)
                subPolys[i].Rotate(deltaRad, position);

            rotation += deltaRad;
            direction = Vec2f.DirectionFrom(rotation);
        }

        public override void Move(float deltaX, float deltaY)
        {
            for (int i = 0; i < subPolys.Count; i++)
                subPolys[i].Move(deltaX, deltaY);

            position.X += deltaX;
            position.Y += deltaY;

            MoveBoundingRectangle();
        }

        public override void ScaleUp(float scaleFactor)
        {
            for (int i = 0; i < subPolys.Count; i++)
            {
                subPolys[i].ScaleUp(scaleFactor, position);
            }

            radius *= scaleFactor;
            scale *= scaleFactor;
        }


        public override void Draw(RenderTarget target)
        {
            for (int i = 0; i < subPolys.Count; i++)
                subPolys[i].Draw(target);

            /*
            CircleShape tmp1 = new CircleShape(radius);
            tmp1.Origin = new Vector2f(radius, radius);
            tmp1.Position = position;
            tmp1.FillColor = new Color(255, 0, 0, 55);

            target.Draw(tmp1);
            */
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {

        }


        private void computerRadius()
        {
            float maxRadius = float.NegativeInfinity;

            foreach (ConvexCollider tmpCol in subPolys)
            {
                for (int i = 0; i < tmpCol.VertexCount; i++)
                {
                    float tmpRadius = Vec2f.EuclidianDistance(position, tmpCol[i]);

                    if (tmpRadius > maxRadius)
                        maxRadius = tmpRadius;
                }
            }

            radius = maxRadius;
        }
    }
}
