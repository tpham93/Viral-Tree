using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public class CircleCollider : ACollider
    {
        private CircleShape shape;

        public CircleCollider(float radius)
            : base(ECollidable.Circle)
        {

            this.radius = radius;
            this.scale = 1.0f;

            shape = new CircleShape(radius);
            shape.Origin = new Vector2f(radius, radius);
            shape.FillColor = Color.Red;

            CalculateBoundingRectangle();
        }

        public override IntersectionData Intersects(ACollider other)
        {
            if (other.CollidableType == ECollidable.Circle)
                return SatCircle(other as CircleCollider);

            else if (other.CollidableType == ECollidable.Convex)
                return SatConvex(other as ConvexCollider);

            else if (other.CollidableType == ECollidable.Concave)
                return (other as ConcaveCollider).SatCircle(this, true);

            else
                return new IntersectionData(false);
        }

        public IntersectionData SatCircle(CircleCollider other)
        {
            CircleCollider otherTmp = (other as CircleCollider);

            float r = radius + otherTmp.radius;

            Vector2f direction = otherTmp.position - position;

            float dist = Vec2f.Length(direction);

            if (dist < r)
                return new IntersectionData(r - dist, Vec2f.Normalized(direction, dist));

            else
                return new IntersectionData(false);
        }

        public IntersectionData SatConvex(ConvexCollider other)
        {
            IntersectionData tmpData = other.SatCircle(this);

            if (tmpData.Intersects)
                tmpData.separationDirections[0] *= -1;

            return tmpData;
        }


        public override void Rotate(float deltaRad)
        {
            rotation += deltaRad;
            direction = Vec2f.DirectionFrom(rotation);
        }

        public override void Move(float deltaX, float deltaY)
        {
            position.X += deltaX;
            position.Y += deltaY;

            MoveBoundingRectangle();
        }

        public override void ScaleUp(float scaleFactor)
        {
            if (scaleFactor > 1.0f || (scaleFactor < 1.0f && scale > MathUtil.ZERO_TOLERANCE))
            {
                radius *= scaleFactor;
                scale *= scaleFactor;
            }
        }



        public override void Draw(RenderTarget target)
        {
            
            shape.Position = Position;
            shape.Rotation = MathUtil.ToDegree(rotation);
            shape.Radius = radius;
            target.Draw(shape);
           
            /*
            CircleShape posiShape = new CircleShape(5.0f);
            posiShape.Origin = new Vector2f(5.0f, 5.0f);
            posiShape.FillColor = new Color(0, 255, 0, 255);
            posiShape.Position = position;

            CircleShape centerShape = new CircleShape(5.0f);
            centerShape.Origin = new Vector2f(5.0f, 5.0f);
            centerShape.FillColor = new Color(255, 0, 0, 255);
            centerShape.Position = Position;

           
            target.Draw(posiShape);

            target.Draw(centerShape);

            RectangleShape rectShape = new RectangleShape(new Vector2f(boundingRect.Width, boundingRect.Height));
            rectShape.Position = new Vector2f(boundingRect.Left, boundingRect.Top);
            rectShape.FillColor = new Color(255, 255, 255, 125);

            target.Draw(rectShape);
            */ 
             
        }

        public override void Draw(RenderTarget target, RenderStates states)
        {
            CircleShape shape = new CircleShape(radius);
            shape.Origin = new Vector2f(radius, radius);
            shape.FillColor = new Color(255, 0, 255, 50);
            shape.Position = Position;

            target.Draw(shape);
        }

        public override ACollider Copy()
        {
            return new CircleCollider(this.radius);
        }
    }
}
