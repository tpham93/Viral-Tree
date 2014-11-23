using SFML.Graphics;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViralTree
{
    public class MyRandom : Random
    {
        public MyRandom()
            : base()
        {

        }

        public MyRandom(int seed)
            : base(seed)
        {

        }

        /// <summary>
        /// Returns a random value between [min, max)
        /// </summary>
        /// <param name="min">Minimum value.</param>
        /// <param name="max">Maximal value, excluding the value itself.</param>
        /// <returns>Value bigger or equal to min and smaller than max.</returns>
        public double NextDouble(double min, double max)
        {
            return NextDouble() * (max - min) + min;
        }


        public float NextFloat()
        {
            return (float)NextDouble();
        }

        public float NextFloat(float min, float max)
        {
            return NextFloat() * (max - min) + min;
        }


        public Vector2f NextVec2fDir()
        {
            double val = Math.PI * 2.0 * NextDouble();
            return new Vector2f((float)Math.Cos(val), (float)Math.Sin(val));
        }

        public Vector2f NextVec2fDir(double radianAngle)
        {
            double val = radianAngle * NextDouble() - radianAngle * 0.5f;
            return Vec2f.DirectionFrom((float)val);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction">Should be normalized for correct computation.</param>
        /// <param name="minLength">Minimal length.</param>
        /// <param name="maxLength">Maximal length.</param>
        /// <returns>Input Vector2f with a length between min and excluding max.</returns>
        public Vector2f RandomLength(Vector2f direction, float minLength, float maxLength)
        {
            return direction * NextFloat(minLength, maxLength);
        }

        public Vector2f NextVec2f(float minX, float maxX, float minY, float maxY)
        {
            return new Vector2f(NextFloat(minX, maxX), NextFloat(minY, maxY));
        }


        public Color RandomColor()
        {
            return new Color((byte)(255 * NextDouble()),
                             (byte)(255 * NextDouble()),
                             (byte)(255 * NextDouble()),
                             (byte)(255 * NextDouble()));
        }

        public Color RandomColor(byte alpha)
        {
            return new Color((byte)(255 * NextDouble()),
                             (byte)(255 * NextDouble()),
                             (byte)(255 * NextDouble()),
                             alpha);
        }

        public Color RandomColor(byte minRed, byte maxRed, byte minGreen, byte maxGree, byte minBlue, byte maxblue, byte alpha)
        {
            return new Color((byte)(NextDouble() * (maxRed - minRed) + minRed),
                             (byte)(NextDouble() * (maxGree - minGreen) + minGreen),
                             (byte)(NextDouble() * (maxblue - minBlue) + minBlue),
                             alpha);
        }


        public Color RandomColor(byte minRed, byte maxRed, byte minGreen, byte maxGree, byte minBlue, byte maxblue, byte minAlpha, byte maxAlpha)
        {
            return new Color((byte)(NextDouble() * (maxRed - minRed) + minRed),
                             (byte)(NextDouble() * (maxGree - minGreen) + minGreen),
                             (byte)(NextDouble() * (maxblue - minBlue) + minBlue),
                             (byte)(NextDouble() * (maxAlpha - minAlpha) + minAlpha));
        }

        /// <summary>
        /// Author: Herb
        /// Returns a random index from 0 to weights.length, with its values as the chance.
        /// </summary>
        /// <param name="weights">Array of chances getting its index. The sum must be 1.0.</param>
        /// <returns>Index of a random value.</returns>
        public int WeightedRandom(double[] weights)
        {
            ValueAndIndex[] weightAndIndex = new ValueAndIndex[weights.Length];

            double sum = 0;
            //copy data in struct and test sum
            for (int i = 0; i < weights.Length; i++)
            {
                weightAndIndex[i] = new ValueAndIndex(weights[i], i);
                sum += weights[i];
            }
            if (sum < 0.9999 || sum > 1.0001)
                throw new Exception("The sum of weights has to be 1 -> 100%\nBut sum is " + sum);

            //sort the ValueAndIndex struct by value: ascending
            Array.Sort<ValueAndIndex>(weightAndIndex, (x, y) => y.value.CompareTo(x.value));

            double randomCounter = NextDouble();

            //test if randomCounter has the right weight
            for (int i = 0; i < weightAndIndex.Length; i++)
            {
                if (randomCounter < weightAndIndex[i].value)
                    return weightAndIndex[i].index;

                randomCounter -= weights[i];
            }
            return weightAndIndex[weightAndIndex.Length - 1].index;
        }


        //Made by Herb
        private struct ValueAndIndex
        {
            public ValueAndIndex(double value, int index)
            {
                this.value = value;
                this.index = index;
            }

            public double value;
            public int index;
        }
    }
}
