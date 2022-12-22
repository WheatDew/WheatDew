using System;
using UnityEngine;
using System.Collections;

namespace WPMF {
    public static class FastVector {

        /// <summary>
        /// Averages two vectors and writes results to another vector
        /// </summary>
        public static void Average(ref Vector3 v1, ref Vector3 v2, ref Vector3 result) {
            result.x = (v1.x + v2.x) * 0.5f;
            result.y = (v1.y + v2.y) * 0.5f;
            result.z = (v1.z + v2.z) * 0.5f;
        }

        /// <summary>
        /// Averages two vectors and writes results to another vector
        /// </summary>
        public static void Average(ref Vector2 v1, ref Vector2 v2, ref Vector2 result) {
            result.x = (v1.x + v2.x) * 0.5f;
            result.y = (v1.y + v2.y) * 0.5f;
        }

        /// <summary>
        /// Substracts one vector to another
        /// </summary>
        public static void Substract(ref Vector3 v1, ref Vector3 v2) {
            v1.x -= v2.x;
            v1.y -= v2.y;
            v1.z -= v2.z;
        }

        /// <summary>
        /// Adds v2 to v1
        /// </summary>
        public static void Add(ref Vector3 v1, ref Vector3 v2) {
            v1.x += v2.x;
            v1.y += v2.y;
            v1.z += v2.z;
        }

        /// <summary>
        /// Adds v2 multiplied by a float value to v1
        /// </summary>
        public static void Add(ref Vector3 v1, ref Vector3 v2, float v) {
            v1.x += v2.x * v;
            v1.y += v2.y * v;
            v1.z += v2.z * v;
        }

        /// <summary>
        /// Writes to result the normalized direction from one position to another position
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="result">Result.</param>
        public static void NormalizedDirection(ref Vector3 from, ref Vector3 to, ref Vector3 result) {
            float dx = to.x - from.x;
            float dy = to.y - from.y;
            float dz = to.z - from.z;
            float length = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
            result.x = dx / length;
            result.y = dy / length;
            result.z = dz / length;
        }

        /// <summary>
        /// Returns the sqr distance from one position to another
        /// </summary>
        public static float SqrDistance(ref Vector3 v1, ref Vector3 v2) {
            float dx = v2.x - v1.x;
            float dy = v2.y - v1.y;
            float dz = v2.z - v1.z;
            return dx * dx + dy * dy + dz * dz;
        }

        /// <summary>
        /// Returns the sqr distance from one position to another. Alternate version that passes vectors by value.
        /// </summary>
        public static float SqrDistanceByValue(Vector3 v1, Vector3 v2) {
            float dx = v2.x - v1.x;
            float dy = v2.y - v1.y;
            float dz = v2.z - v1.z;
            return dx * dx + dy * dy + dz * dz;
        }


        /// <summary>
        /// Returns the sqr distance from one position to another
        /// </summary>
        public static float SqrDistance(ref Vector2 v1, ref Vector2 v2) {
            float dx = v2.x - v1.x;
            float dy = v2.y - v1.y;
            return dx * dx + dy * dy;
        }


    }
}