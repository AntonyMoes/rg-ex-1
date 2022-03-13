using Math;

namespace Physics.Calculators {
    public class OrbitingSimplifiedCalculator : IBodySimplifiedCalculator {
        private readonly int _direction;
        private readonly double _distance;
        private readonly float _initialAngle;
        private readonly PhysicsBody _orbitingTarget;

        public OrbitingSimplifiedCalculator(PhysicsBody orbitingTarget, Vector2Double initialPosition, bool clockWise) {
            _orbitingTarget = orbitingTarget;
            _direction = clockWise ? 1 : -1;

            var vector = initialPosition - _orbitingTarget.Position;
            _distance = vector.magnitude;
            _initialAngle = (float) Vector2Double.SignedAngle(Vector2Double.up, vector);
        }

        public double Time { get; set; }

        public void CalculateProperties(double gravityConstant, out Vector2Double position, out float rotation) {
            var orbitTime = 2d * System.Math.PI *
                            System.Math.Sqrt(System.Math.Pow(_distance, 3d) / (gravityConstant * _orbitingTarget.Mass));
            var currentTime = Time % orbitTime / orbitTime;
            var currentAngle = (_initialAngle + 360d * currentTime) % 360d;

            var vector = Vector2Double.Rotate(Vector2Double.up * _distance, currentAngle * _direction);
            position = _orbitingTarget.Position + vector;
            rotation = (float) currentAngle;
        }
    }
}
