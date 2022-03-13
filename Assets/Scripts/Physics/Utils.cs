using Math;
using UnityEngine;

namespace Physics {
    public static class Utils {
        public static Vector2Double GameToPhysicsPosition(Vector3 position) {
            return (Vector2) position;
        }

        public static Vector3 PhysicsToGamePosition(Vector2Double position, float z) {
            return (Vector3) (Vector2) position + Vector3.forward * z;
        }

        public static float GameToPhysicsRotation(Quaternion rotation) {
            return -rotation.eulerAngles.y;
        }

        public static Quaternion PhysicsToGameRotation(float rotation) {
            return Quaternion.Euler(0f, -rotation, 0f);
        }

        public static double CalculateGravity(double gravityConstant, double massA, Vector2Double positionA,
            double massB, Vector2Double positionB) {
            return gravityConstant * massA * massB /
                   System.Math.Pow((positionA - positionB).magnitude, 2d);
        }
    }
}
