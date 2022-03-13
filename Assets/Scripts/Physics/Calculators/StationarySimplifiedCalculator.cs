using Math;

namespace Physics.Calculators {
    public class StationarySimplifiedCalculator : IBodySimplifiedCalculator {
        private readonly float _angleVelocity;
        private readonly Vector2Double _position;

        public StationarySimplifiedCalculator(Vector2Double position, float angleVelocity) {
            _position = position;
            _angleVelocity = angleVelocity;
        }

        public double Time { get; set; }

        public void CalculateProperties(double gravityConstant, out Vector2Double position, out float rotation) {
            position = _position;
            rotation = (float) (Time * _angleVelocity) % 360f;
        }
    }
}
