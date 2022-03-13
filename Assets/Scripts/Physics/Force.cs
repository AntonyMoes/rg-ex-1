using Math;
using Values;

namespace Physics {
    public class Force {
        private readonly Vector2Double? _position;
        private readonly bool _relative;
        private readonly Vector2Double _vector;

        public Force(Vector2Double vector, Vector2Double? position = null, bool relative = true) {
            _vector = vector;
            _position = position;
            _relative = relative;
        }

        public Vector2Double GetAbsolutePosition(Value<Vector2Double> targetPosition) {
            return _position == null
                ? targetPosition.Val
                : _relative
                    ? _position.Value + targetPosition
                    : _position.Value;
        }

        public Vector2Double GetAbsoluteVector(Value<float> targetRotation) {
            return _relative
                ? Vector2Double.Rotate(_vector, targetRotation)
                : _vector;
        }
    }
}
