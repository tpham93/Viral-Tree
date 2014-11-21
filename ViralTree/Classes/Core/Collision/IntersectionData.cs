
using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    /// <summary>
    /// Some data that contains information about the collision between two Polygons.
    /// </summary>
    public struct IntersectionData
    {
        //TODO: maybe add references to the colliding polygons.

        /// <summary>
        /// Tells if the given objects even intersect.
        /// </summary>
        public bool Intersects
        {
            get { return peneValues != null; }
        }

        /// <summary>
        /// Is the depth of penetration.
        /// </summary>
        public List<float> peneValues;

        /// <summary>
        /// Is the direction the intersection can be resolved with when multiplied with the penetrationValue.
        /// </summary>
        public List<Vector2f> separationDirections;


        /// <summary>
        /// Creates the collision data with intersection.
        /// </summary>
        /// <param name="penetrationValue">The depth of penetration.</param>
        /// <param name="separationDirection">The direction the intersection between two Polygons can be resolved.</param>
        public IntersectionData(float penetrationValue, Vector2f separationDirection)
        {
            this.peneValues = new List<float>();
            this.peneValues.Add(penetrationValue);

            this.separationDirections = new List<Vector2f>();
            this.separationDirections.Add(separationDirection);
        }

        /// <summary>
        /// Separates a and b with aMount and bAmount
        /// </summary>
        public void Seperate(ACollider a, ACollider b, float aAmount, float bAmount)
        {
            for (int i = 0; i < peneValues.Count; i++)
            {
                //TODO: maybe swap - from aAmount to bAmount:
                a.Move(separationDirections[i] * peneValues[i] * -aAmount);
                b.Move(separationDirections[i] * peneValues[i] * bAmount);
            }
        }

        /// <summary>
        /// Separates a and b with aMount and bAmount
        /// </summary>
        public void Seperate(ACollider a, float aAmount)
        {
            for (int i = 0; i < peneValues.Count; i++)
                a.Move(separationDirections[i] * peneValues[i] * -aAmount);
        }
    }
}
