using Math;
using Physics.Calculators;
using Values;

namespace Physics {
    public class PhysicsBody {
        public readonly UpdatedValue<float> AngularVelocity;
        public readonly UpdatedValue<double> Mass;
        public readonly UpdatedValue<Vector2Double> Position;
        public readonly UpdatedValue<float> Rotation;

        public readonly IBodySimplifiedCalculator SimplifiedCalculator;

        // public readonly UpdatedValue<bool> Stationary;
        public readonly UpdatedValue<Vector2Double[]> Trajectory;
        public readonly UpdatedValue<Vector2Double> Velocity;

        private IForceApplier _applier;

        public PhysicsBody(Value<double> mass, Value<Vector2Double> position, Value<Vector2Double> velocity,
            Value<float> rotation, Value<float> angularVelocity,
            IBodySimplifiedCalculator simplifiedCalculator = null) {
            Mass = new UpdatedValue<double>(mass);
            Position = new UpdatedValue<Vector2Double>(position);
            Velocity = new UpdatedValue<Vector2Double>(velocity);
            Rotation = new UpdatedValue<float>(rotation);
            AngularVelocity = new UpdatedValue<float>(angularVelocity);
            Trajectory = new UpdatedValue<Vector2Double[]>();
            SimplifiedCalculator = simplifiedCalculator;
        }

        public void SetForceApplier(IForceApplier applier) {
            _applier = applier;
        }

        public void AddForce(Force force) {
            _applier?.AddForce(this, force);
        }

        public void RemoveForce(Force force) {
            _applier?.RemoveForce(this, force);
        }

        public void AddForceImmediate(Force force) {
            _applier?.AddForceImmediate(this, force);
        }

        public PhysicsBody Clone() {
            return new PhysicsBody(Mass.Val, Position.Val, Velocity.Val,
                Rotation.Val, AngularVelocity.Val);
        }
    }
}
