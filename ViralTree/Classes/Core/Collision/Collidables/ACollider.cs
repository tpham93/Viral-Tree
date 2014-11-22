using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public abstract class ACollider
    {
        public enum ECollidable { Circle, Convex, Concave, Count };

        protected Vector2f position;
        protected Vector2f direction = Vec2f.UnitX;

        protected float rotation = 0;
        protected float scale;
        protected float radius;

        protected ECollidable colType;

        protected FloatRect boundingRect;

        public Vector2f Position
        {
            get { return position; }
            set { Move(value - position); }
        }
        public Vector2f Direction
        {
            get { return direction; }
            set { Rotate(Vec2f.RotationFrom(value) - rotation); }
        }

        public float Rotation
        {
            get { return rotation; }
            set { Rotate(value - rotation); }
        }
        public float Scale
        {
            get { return scale; }
            set { ScaleUp(value / scale); CalculateBoundingRectangle(); }
        }
        public float Radius
        {
            get { return radius; }
            set { ScaleUp(value / radius); CalculateBoundingRectangle(); }
        }

        public ECollidable CollidableType
        {
            get { return colType; }
            private set { colType = value; }
        }

        public FloatRect BoundingRectangle
        {
            get { return boundingRect; }
            private set { boundingRect = value; }
        }


        public ACollider(ECollidable type)
        {
            colType = type;
        }


        public abstract IntersectionData Intersects(ACollider other);

        public bool IntersectsBounding(ACollider other)
        {
            float r = radius + other.radius;

            return Vec2f.EuclidianDistanceSq(position, other.position) < r * r;
        }


        public abstract void Rotate(float deltaRad);

        public void Move(Vector2f deltaPos)
        {
            Move(deltaPos.X, deltaPos.Y);
        }

        public abstract void Move(float deltaX, float deltaY);

        public abstract void ScaleUp(float scaleFactor);


        public abstract void Draw(RenderTarget target);

        public abstract void Draw(RenderTarget target, RenderStates states);

        public void CalculateBoundingRectangle()
        {
            boundingRect.Left = position.X -  radius;
            boundingRect.Top = position.Y - radius;

            boundingRect.Width = radius*2;
            boundingRect.Height = radius*2;
        }

        public void MoveBoundingRectangle()
        {
            boundingRect.Left = position.X - radius;
            boundingRect.Top = position.Y - radius;
        }

    }
}
