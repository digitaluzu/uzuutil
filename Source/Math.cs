using UnityEngine;
using System.Collections;

namespace Uzu
{
    /// <summary>
    /// Math related services.
    /// </summary>
    public static class Math
    {
        public const float TWO_PI = 2.0f * Mathf.PI;
        public const float PI_2 = Mathf.PI * 0.5f;
        public const float E = 2.71828182846f;

        /// <summary>
        /// Returns a vector with the z component equal to 0.
        /// </summary>
        public static Vector3 FlattenZ (Vector3 inVec)
        {
            return new Vector3 (inVec.x, inVec.y, 0);
        }

        /// <summary>
        /// Returns a vector with the x, y, z components the same.
        /// </summary>
        public static Vector3 FloatToVector3 (float inVal)
        {
            return new Vector3 (inVal, inVal, inVal);
        }

        /// <summary>
        /// Turns a Vector3 into a Vector2 (drops the z coordinate).
        /// </summary>
        public static Vector2 Vector3ToVector2 (Vector3 inVec)
        {
            return new Vector2 (inVec.x, inVec.y);
        }

        /// <summary>
        /// Creates a new Vector3 from a Vector2, using 0 for the z component.
        /// </summary>
        public static Vector3 Vector2ToVector3 (Vector2 inVec)
        {
            return new Vector3 (inVec.x, inVec.y, 0.0f);
        }

        /// <summary>
        /// Creates a new Vector3 from a Vector2 and a z component.
        /// </summary>
        public static Vector3 Vector2ToVector3 (Vector2 inVec, float z)
        {
            return new Vector3 (inVec.x, inVec.y, z);
        }

        /// <summary>
        /// Turns a Vector4 into a Vector3 (dropping the w component).
        /// </summary>
        public static Vector3 Vector4ToVector3 (Vector4 inVec)
        {
            return new Vector3 (inVec.x, inVec.y, inVec.z);
        }

        /// <summary>
        /// Creates a new Vector4 from a Vector3 and a w component.
        /// </summary>
        public static Vector4 Vector3ToVector4 (Vector3 inVec, float w)
        {
            return new Vector4 (inVec.x, inVec.y, inVec.z, w);
        }

        /// <summary>
        /// Returns the min element (x, y, or z) of the vector.
        /// </summary>
        public static float VectorMinElement( Vector3 v)
        {
            return Mathf.Min (v.z, Mathf.Min (v.x, v.y));
        }

        /// <summary>
        /// Returns the max element (x, y, or z) of the vector.
        /// </summary>
        public static float VectorMaxElement( Vector3 v)
        {
            return Mathf.Max (v.z, Mathf.Max (v.x, v.y));
        }

        /// <summary>
        /// Clamps a vector to a given min/max range.
        /// </summary>
        public static Vector3 Clamp (Vector3 v, Vector3 min, Vector3 max)
        {
            return new Vector3 (Mathf.Clamp (v.x, min.x, max.x), Mathf.Clamp (v.y, min.y, max.y), Mathf.Clamp (v.z, min.z, max.z));
        }

        /// <summary>
        /// Generates a random vector on a unit circle.
        /// </summary>
        public static Vector2 RandomOnUnitCircle ()
        {
            return RadiansToDirectionVector2 (Random.value * TWO_PI);
        }

        /// <summary>
        /// Generates a random number within the given range.
        /// </summary>
        public static float RandomRange (Vector2 range)
        {
            return Random.Range (range.x, range.y);
        }

        /// <summary>
        /// Returns a random sign (-1 or 1).
        /// </summary>
        public static int RandomSign ()
        {
            return (Random.Range (0, 2) == 0) ? -1 : 1;
        }

        /// <summary>
        /// Converts an angle into a direction around a unit circle.
        /// </summary>
        public static Vector3 RadiansToDirectionVector (float angleInRadians)
        {
            Vector2 v = RadiansToDirectionVector2 (angleInRadians);
            return new Vector3 (v.x, v.y, 0.0f);
        }

        /// <summary>
        /// Converts an angle into a direction (2D) around a unit circle.
        /// </summary>
        public static Vector2 RadiansToDirectionVector2 (float angleInRadians)
        {
            return new Vector2 (Mathf.Cos (angleInRadians), Mathf.Sin (angleInRadians));
        }

        /// <summary>
        /// Given segment (ab) and point (c), computest closest point (d) on (ab).
        /// </summary>
        public static void ClosestPtPointSegment (Vector3 a, Vector3 b, Vector3 c, out Vector3 d)
        {
            float t;
            ClosestPtPointSegment (a, b, c, out t, out d);
        }

        /// <summary>
        /// Given segment (ab) and point (c), computest closest point (d) on (ab).
        /// Also returns t for the parametric position of (d): d(t) = a + t*(b - a).
        /// </summary>
        public static void ClosestPtPointSegment (Vector3 a, Vector3 b, Vector3 c, out float t, out Vector3 d)
        {
            Vector3 ab = b - a;

            // Project c onto ab, but deferring divide by Dot(ab, ab)
            t = Vector3.Dot (c - a, ab);
            if (t <= 0.0f) {
                // c projects outside the [a,b] interval, on the a side; clamp to a
                t = 0.0f;
                d = a;
            } else {
                float denom = Vector3.Dot (ab, ab); // Always nonnegative since denom = ||ab||∧2
                if (t >= denom) {
                    // c projects outside the [a,b] interval, on the b side; clamp to b
                    t = 1.0f;
                    d = b;
                } else {
                    // c projects inside the [a,b] interval; must do deferred divide now
                    t = t / denom;
                    d = a + t * ab;
                }
            }
        }

        /// <summary>
        /// Returns the squared distance between point (c) and segment (ab).
        /// </summary>
        public static float SqDistPointSegment (Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 ab = b - a;
            Vector3 ac = c - a;
            Vector3 bc = c - b;

            float e = Vector3.Dot (ac, ab);

            // Handle cases where c projects outside ab
            if (e <= 0.0f) {
                return Vector3.Dot (ac, ac);
            }

            float f = Vector3.Dot (ab, ab);
            if (e >= f) {
                return Vector3.Dot (bc, bc);
            }

            // Handle cases where c projects onto ab
            return Vector3.Dot (ac, ac) - e * e / f;
        }

        #region Primitive intersections.
        /// <summary>
        /// Given a point p and a sphere,
        /// return whether the point intersects the sphere.
        /// </summary>
        public static bool IntersectPointSphere (Vector3 p, Sphere s)
        {
            float r2 = s.Radius * s.Radius;
            float distSqr = Vector3.SqrMagnitude(p - s.Center);
            return distSqr <= r2;
        }

        /// <summary>
        /// Given the line segment ab, and plane p, return whether
        /// the segment intersects the plane.
        /// </summary>
        public static bool IntersectSegmentPlane (Vector3 a, Vector3 b, Plane p, out float t, out Vector3 q)
        {
            // Ref: Real-Time Collision Detection

            // Compute the t value for the directed line ab intersecting the plane.
            Vector3 ab = b - a;
            t = (p.distance - Vector3.Dot (p.normal, a)) / Vector3.Dot (p.normal, ab);

            // If t in [0..1] compute and return intersection point.
            if (t >= 0.0f && t <= 1.0f) {
                q = a + t * ab;
                return true;
            }
            // Else no intersection
            else {
                q = Vector3.zero;
                return false;
            }
        }

        /// <summary>
        /// Given line pq and cw quadrilateral abcd, return whether the line
        /// pierces the quadrilateral. If so, also return the point r of intersection
        /// </summary>
        public static bool IntersectLineQuad(Vector3 p, Vector3 q, Vector3 a, Vector3 b, Vector3 c, Vector3 d, out Vector3 r)
        {
            r = Vector3.zero;

            Vector3 pq = q - p;
            Vector3 pa = a - p;
            Vector3 pb = b - p;
            Vector3 pc = c - p;

            // Determine which triangle to test against by testing against diagonal first.
            Vector3 m = Vector3.Cross(pc, pq);
            float v = Vector3.Dot(pa, m);    // ScalarTriple(pq, pa, pc);
            if (v >= 0.0f) {
                // Test intersection against triangle abc.
                float u = -Vector3.Dot(pb, m);    // ScalarTriple(pq, pc, pb);
                if (u < 0.0f) {
                    return false;
                }
                float w = ScalarTriple(pq, pb, pa);
                if (w < 0.0f) {
                    return false;
                }
                // Compute r, r = u*a + v*b + w*c, from barycentric coordinates (u, v, w).
                float denom = 1.0f / (u + v + w);
                u *= denom;
                v *= denom;
                w *= denom;
                r = u * a + v * b + w * c;
            }
            else {
                // Test intersection against triangle dac.
                Vector3 pd = d - p;
                float u = Vector3.Dot(pd, m);    // ScalarTriple(pq, pd, pc);
                if (u < 0.0f) {
                    return false;
                }
                float w = ScalarTriple(pq, pa, pd);
                if (w < 0.0f) {
                    return false;
                }
                v = -v;
                // Compute r, r = u*a + v*d + w*c, from barycentric coordinates (u, v, w).
                float denom = 1.0f / (u + v + w);
                u *= denom;
                v *= denom;
                w *= denom;    // w = 1.0f - u - v;
                r = u * a + v * d + w * c;
            }

            return true;
        }

        /// <summary>
        /// Intersects a segment and a sphere.
        /// </summary>
        public static bool IntersectSegmentSphere(Vector3 p0, Vector3 p1, Sphere s, out Vector3 hitPos)
        {
            Vector3 dir = p1 - p0;
            float length = dir.magnitude;

            if (Mathf.Approximately(length, 0.0f)) {
                hitPos = p0;
                return IntersectPointSphere(p0, s);
            }

            dir = dir / length; // Normalize

            // Perform ray/sphere intersection, and verify t to guarantee
            // collision occurred within boundaries of segment.
            float t;
            if (IntersectRaySphere(p0, dir, s, out t, out hitPos)) {
                if (t <= length) {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Intersects ray r = p + td, |d| = 1, with sphere s and, if intersecting,
        /// returns t value of intersection and intersection point q
        /// </summary>
        public static bool IntersectRaySphere(Vector3 p, Vector3 d, Sphere s, out float t, out Vector3 q)
        {
            // Ref: Real-Time Collision Detection

            Vector3 m = p - s.Center;
            float b = Vector3.Dot(m, d);
            float c = Vector3.Dot(m, m) - s.Radius * s.Radius;
            // Exit if r’s origin outside s (c > 0) and r pointing away from s (b > 0)
            if (c > 0.0f && b > 0.0f) {
                t = 0.0f;
                q = Vector3.zero;
                return false;
            }
            float discr = b * b - c;
            // A negative discriminant corresponds to ray missing sphere
            if (discr < 0.0f) {
                t = 0.0f;
                q = Vector3.zero;
                return false;
            }
            // Ray now found to intersect sphere, compute smallest t value of intersection
            t = -b - Mathf.Sqrt(discr);
            // If t is negative, ray started inside sphere so clamp t to zero
            if (t < 0.0f) {
                t = 0.0f;
            }
            q = p + t * d;
            return true;
        }

        /// <summary>
        /// Intersects a ray and sphere: r = p + td
        /// </summary>
        public static bool IntersectRaySphere(Vector3 p, Vector3 d, Sphere s)
        {
            // Ref: Real-Time Collision Detection

            Vector3 m = p - s.Center;
            float c = Vector3.Dot(m, m) - s.Radius * s.Radius;
            // If there is definitely at least one real root, there must be an intersection
            if (c <= 0.0f) {
                return true;
            }
            float b = Vector3.Dot(m, d);
            // Early exit if ray origin outside sphere and ray pointing away from sphere
            if (b > 0.0f) {
                return false;
            }
            float disc = b * b - c;
            // A negative discriminant corresponds to ray missing sphere
            if (disc < 0.0f) {
                return false;
            }
            // Now ray must hit sphere
            return true;
        }

        /// <summary>
        /// Compute the "triple product" of the three vectors.
        /// http://en.wikipedia.org/wiki/Triple_product
        /// </summary>
        private static float ScalarTriple(Vector3 u, Vector3 v, Vector3 w)
        {
            return Vector3.Dot(u, Vector3.Cross(v, w));
        }
        #endregion
    }
}
