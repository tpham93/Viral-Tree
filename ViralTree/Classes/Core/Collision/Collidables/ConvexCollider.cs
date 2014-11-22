using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public class ConvexCollider : ACollider
    {
        //____________MEMBER / GETTER / SETTER________________

        private VertexArray vertices;
        public Vector2f this[int index]
        {
            get { return vertices[(uint)index + 1].Position; }
            set
            {
                uint indx = (uint)(index + 1);
                Vertex tmp = vertices[indx];
                tmp.Position = value;
                vertices[indx] = tmp;
            }
        }

        private Matrix2D transformMatrix;
        private Vector2f[] normals;

        private bool redundantNormals = false;

        public Vector2f GetNormal(int index)
        {
            return normals[index];
        }
        public void SetNormal(int index, Vector2f normal)
        {

            System.Diagnostics.Debug.Assert(MathUtil.NearEqual(Vec2f.Length(normal), 1.0f));

            normals[index] = normal;
        }


        public void SetTextureCoord(uint index, Vector2f pos)
        {
            Vertex tmp = this.vertices[index];

            tmp.TexCoords = pos;

            this.vertices[index] = tmp;
        }

        public int VertexCount
        {
            get;
            private set;
        }



        //______________CONSTRUCTOR_________________________ 

        public ConvexCollider(Vector2f[] vertices)
            : base(ECollidable.Convex)
        {
            //+ 2 for common center and the second Vector2f twice to close the "gap"
            this.vertices = new VertexArray(PrimitiveType.TrianglesFan, (uint)vertices.Length + 2);

            transformMatrix = Matrix2D.Identity;

            for (uint i = 0; i < vertices.Length; i++)
            {
                this.vertices[i + 1] = new Vertex(vertices[i], Color.Red);
                position += vertices[i];
            }

            //Compute the center of all vertices:
            position /= vertices.Length;

            //Set common center for TrianglesFan:
            this.vertices[0] = new Vertex(position, Color.Green);

            //Set last Vertex (closing the gap)
            this.vertices[this.vertices.VertexCount - 1] = new Vertex(vertices[0], Color.Red);

            VertexCount = vertices.Length;

            normals = new Vector2f[VertexCount];

            computeNormals();
            computeRadius();
            CalculateBoundingRectangle();
        }


        public ConvexCollider(Vector2f[] vertices, bool redundant)
            : this(vertices)
        {
            this.redundantNormals = redundant;
        }


        //______________TRANSFORM METHODS_________________________

        public override void Rotate(float deltaRad)
        {
            Rotate(deltaRad, position);
        }

        public override void Move(float deltaX, float deltaY)
        {
            for (uint i = 0; i < vertices.VertexCount; i++)
            {
                Vertex tmpVertex = this.vertices[i];

                tmpVertex.Position.X += deltaX;
                tmpVertex.Position.Y += deltaY;

                this.vertices[i] = tmpVertex;
            }

            position.X += deltaX;
            position.Y += deltaY;

            MoveBoundingRectangle();
        }

        public override void ScaleUp(float scaleFactor)
        {
            ScaleUp(scaleFactor, position);
        }



        public void Rotate(float deltaRad, Vector2f rotaOrigin)
        {

            transformMatrix.convertToRotation(deltaRad);

            for (uint i = 0; i < vertices.VertexCount; i++)
            {
                Vertex tmpVertex = this.vertices[i];

                tmpVertex.Position = transformMatrix.mult(tmpVertex.Position - rotaOrigin) + rotaOrigin;

                this.vertices[i] = tmpVertex;

                if (i < VertexCount)
                    normals[i] = transformMatrix.mult(normals[i]);

            }

            if (!Vec2f.NearEqual(position, rotaOrigin))
                position = transformMatrix.mult(position - rotaOrigin) + rotaOrigin;

            rotation += deltaRad;
            direction = Vec2f.DirectionFrom(rotation);

        }

        public void ScaleUp(float scaleFactor, Vector2f scaleOrigin)
        {
            transformMatrix.convertToScale(scaleFactor, scaleFactor);

            for (uint i = 0; i < vertices.VertexCount; i++)
            {
                Vertex tmpVertex = this.vertices[i];

                tmpVertex.Position = transformMatrix.mult(tmpVertex.Position - scaleOrigin) + scaleOrigin;

                this.vertices[i] = tmpVertex;
            }

            if (!Vec2f.NearEqual(scaleOrigin, position))
                position = transformMatrix.mult(position - scaleOrigin) + scaleOrigin;

            radius *= scaleFactor;
        }

        //___________________DRAWING METHODS_______________

        public override void Draw(RenderTarget target)
        {
            target.Draw(vertices);

            /*
            for (int i = 0; i < VertexCount; i++)
            {
                CircleShape tmpShape = new CircleShape(5.0f);
                tmpShape.Origin = new Vector2f(5.0f, 5.0f);
                tmpShape.Position = this[i];

                target.Draw(tmpShape);
            }
            */

            /*
            CircleShape radiusShape = new CircleShape(radius);
            radiusShape.Origin = new Vector2f(radius, radius);
            radiusShape.Position = position;
            radiusShape.FillColor = new Color(255, 0, 0, 55);

            target.Draw(radiusShape);
            */

            //position:

            /*
            CircleShape posShape = new CircleShape(5.0f);
            posShape.Origin = new Vector2f(5.0f, 5.0f);
            posShape.Position = position;
            posShape.FillColor = new Color(255, 255, 0, 255);

            target.Draw(posShape);
            */

            ///bounding Rectanle:

           /*
            RectangleShape rectShape = new RectangleShape(new Vector2f(boundingRect.Width, boundingRect.Height));
            rectShape.Position = new Vector2f(boundingRect.Left, boundingRect.Top);
            rectShape.FillColor = new Color(255, 255, 255, 125);

            target.Draw(rectShape);
          */
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(vertices, states);


          
        }


        //___________________INTERSECTION METHODS_______________

        public override IntersectionData Intersects(ACollider other)
        {
            if (other.CollidableType == ECollidable.Convex)
                return SatConvex(other as ConvexCollider);

            else if (other.CollidableType == ECollidable.Circle)
                return SatCircle(other as CircleCollider);

            else if (other.CollidableType == ECollidable.Concave)
                return (other as ConcaveCollider).SatConvex(this, true);


            return new IntersectionData();
        }

        //TODO: optimaze, make more readable:
        public IntersectionData SatCircle(CircleCollider other)
        {
            if (this.IntersectsBounding(other))
            {
                float depth = float.PositiveInfinity;
                Vector2f dir = Vec2f.Zero;

                float nearestDistance = float.PositiveInfinity;
                Vector2f nearestPos = Vec2f.Zero;


                int counter = redundantNormals ? VertexCount / 2 : VertexCount;
                for (int i = 0; i < VertexCount; i++)
                {
                    float tmpDist = Vec2f.EuclidianDistance(this[i], other.Position);

                    if (tmpDist < nearestDistance)
                    {
                        nearestDistance = tmpDist;
                        nearestPos = this[i];
                    }

                    //save half the computations when the polygon has normals twice
                    if (!redundantNormals || i < counter)
                    {
                        float aMin = 0;
                        float aMax = 0;

                        findMinMax(ref aMin, ref aMax, ref normals[i]);

                        float bMin = Vec2f.Dot(other.Position - normals[i] * other.Radius, normals[i]);
                        float bMax = Vec2f.Dot(other.Position + normals[i] * other.Radius, normals[i]);

                        if (aMin > bMax || bMin > aMax)
                            return new IntersectionData();

                        float help = aMax == bMax ? Math.Max(aMin - aMax, bMin - bMax) : aMax > bMax ? aMin - bMax : bMin - aMax;

                        if (help < 0)
                            help = -help;

                        if (help < depth)
                        {
                            depth = help;
                            dir = -normals[i];
                        }
                    }
                }

                //projection to the nearest point to the center of the circle:
                Vector2f tmpDir = Vec2f.Normalized(other.Position - nearestPos, nearestDistance);

                float aMin_ = 0;
                float aMax_ = 0;

                findMinMax(ref aMin_, ref aMax_, ref tmpDir);

                float bMin_ = Vec2f.Dot(other.Position - tmpDir * other.Radius, tmpDir);
                float bMax_ = Vec2f.Dot(other.Position + tmpDir * other.Radius, tmpDir);

                if (aMin_ > bMax_ || bMin_ > aMax_)
                    return new IntersectionData();

                float help_ = aMax_ == bMax_ ? Math.Max(aMin_ - aMax_, bMin_ - bMax_) : aMax_ > bMax_ ? aMin_ - bMax_ : bMin_ - aMax_;

                if (help_ < 0)
                    help_ = -help_;

                if (help_ < depth)
                {
                    depth = help_;
                    dir = -tmpDir;
                }

                //direction and depth found now


                if (Vec2f.Dot(dir, other.Position - position) < 0)
                    dir = -dir;

                return new IntersectionData(depth, dir);
            }

            else
                return new IntersectionData();
        }

        public IntersectionData SatConvex(ConvexCollider other)
        {
            if (this.IntersectsBounding(other))
            {

                float peneValue = float.PositiveInfinity;
                Vector2f sepaNormal = new Vector2f(0, 0);


                int counter = redundantNormals ? VertexCount / 2 : VertexCount;
                for (int i = 0; i < counter; i++)
                {
                    float aMin = 0;
                    float aMax = 0;
                    findMinMax(ref aMin, ref aMax, ref normals[i]);

                    float bMin = 0;
                    float bMax = 0;
                    other.findMinMax(ref bMin, ref bMax, ref normals[i]);

                    if (aMin > bMax || bMin > aMax)
                        return new IntersectionData();

                    float help = aMax == bMax ? Math.Max(aMin - aMax, bMin - bMax) : aMax > bMax ? aMin - bMax : bMin - aMax;

                    if (help < 0)
                        help = -help;

                    if (help < peneValue)
                    {
                        peneValue = help;
                        sepaNormal = -normals[i];
                    }

                }

                //same as above for the normals of the second polygon
                int counter2 = other.redundantNormals ? other.VertexCount / 2 : other.VertexCount;
                for (int i = 0; i < counter2; i++)
                {
                    float aMin = 0;
                    float aMax = 0;

                    float bMin = 0;
                    float bMax = 0;

                    findMinMax(ref aMin, ref aMax, ref other.normals[i]);
                    other.findMinMax(ref bMin, ref bMax, ref other.normals[i]);

                    if (aMin > bMax || bMin > aMax)
                        return new IntersectionData();

                    float help = aMax == bMax ? Math.Max(aMin - aMax, bMin - bMax) : aMax > bMax ? aMin - bMax : bMin - aMax;

                    if (help < 0)
                        help = -help;

                    if (help < peneValue)
                    {
                        peneValue = help;
                        sepaNormal = -other.normals[i];
                    }
                }

                if (Vec2f.Dot(sepaNormal, other.position - position) < 0)
                    sepaNormal = -sepaNormal;

                return new IntersectionData(peneValue, sepaNormal);
            }

            else
                return new IntersectionData();
        }


        //__________________PRIVATE METHODS_________________________

        private void findMinMax(ref float min, ref float max, ref Vector2f normal)
        {
            min = float.PositiveInfinity;
            max = float.NegativeInfinity;

            for (int i = 0; i < this.VertexCount; i++)
            {
                float current = Vec2f.Dot(this[i], normal);

                if (current < min)
                    min = current;

                if (current > max)
                    max = current;
            }
        }

        private void computeNormals()
        {
            for (int i = 0; i < VertexCount; i++)
            {
                normals[i] = Vec2f.RightNormal(this[(i + 1) % VertexCount] - this[i]);
            }
        }

        private void computeRadius()
        {
            float tmpRadius = float.NegativeInfinity;

            for (int i = 0; i < VertexCount; i++)
            {
                float tmpDist = Vec2f.EuclidianDistance(position, this[i]);

                if (tmpDist > tmpRadius)
                    tmpRadius = tmpDist;

            }

            radius = tmpRadius;
        }


        public override ACollider Copy()
        {
            Vector2f[] copiedVertices = new Vector2f[this.vertices.VertexCount];
            for(uint i = 0; i < copiedVertices.Length; ++i)
            {
                copiedVertices[i] = this.vertices[i].Position - this.Position;
            }

            return new ConvexCollider(copiedVertices);
        }
    }
}
