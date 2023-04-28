using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace rth_jet_story
{
    class VectorMath
    {
        public static Vector2 line2D(Vector2 a, Vector2 b)
        {
            return new Vector2(b.X - a.X, b.Y - a.Y);
        }

        /**
         *
         * @param p1 - bod lezici na primce 1
         * @param v1 - smerovy vektor primky 1
         * @param p2 - bod lezici na primce 2
         * @param v2 - smerovy vektor primky 2
         * @return - prusecik
         */
        public static bool pointOfIntersection_2Lines2D(
                Vector2 p1, Vector2 v1,
                Vector2 p2, Vector2 v2,
                ref Vector2 result)
        {
            float a1 = -v1.Y;
            float b1 = v1.X;
            float c1 = -(a1 * p1.X) - (b1 * p1.Y);

            float a2 = -v2.Y;
            float b2 = v2.X;
            float c2 = -(a2 * p2.X) - (b2 * p2.Y);

            float tmp = a1 * b2 - b1 * a2;
            if (tmp != 0)
            {
                result = new Vector2(
                        (b1 * c2 - c1 * b2) / tmp,
                        (c1 * a2 - a1 * c2) / tmp
                        );
                return true;
            }

            return false;
        }

        double distanceBetween2Points2D(Vector2 p1, Vector2 p2)
        {
            double v1 = Math.Abs(p2.X - p1.X);
            double v2 = Math.Abs(p2.Y - p1.Y);
            return Math.Sqrt(v1 * v1 + v2 * v2);
        }

        float angleBetween2Vectors2D(Vector2 p1, Vector2 p2)
        {
            float a = Vector2.Dot(p1, p2);
            float b = p1.Length() * p2.Length();
            return (a / b);
        }
    }
}
