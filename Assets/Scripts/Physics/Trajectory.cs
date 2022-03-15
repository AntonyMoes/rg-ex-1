using System;
using Math;

namespace Physics {
    public class Trajectory {
        public Vector2Double[] Path { get; }

        public Trajectory(int length) {
            Path = new Vector2Double[length];
        }

        public void UpdatePath(Vector2Double lastPoint) {
            Array.Copy(Path, 1, Path, 0, Path.Length - 1);
            Path[Path.Length - 1] = lastPoint;
        }

        public void SetPath(Vector2Double[] path) {
            Array.Copy(path, Path, Path.Length);
        }
    }
}