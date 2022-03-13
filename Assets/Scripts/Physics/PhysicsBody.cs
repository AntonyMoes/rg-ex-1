using Math;
using Values;

namespace Physics {
    public class PhysicsBody {
        public readonly UpdatedValue<double> Mass;
        public readonly UpdatedValue<Vector2Double> Position;
        public readonly UpdatedValue<float> Rotation;
        public readonly UpdatedValue<bool> Stationary;
        public readonly UpdatedValue<Vector2Double[]> Trajectory;
        public readonly UpdatedValue<Vector2Double> Velocity;

        public PhysicsBody(Value<double> mass, Value<Vector2Double> position, Value<float> rotation,
            Value<Vector2Double> velocity, Value<bool> stationary) {
            Mass = new UpdatedValue<double>(mass);
            Position = new UpdatedValue<Vector2Double>(position);
            Rotation = new UpdatedValue<float>(rotation);
            Velocity = new UpdatedValue<Vector2Double>(velocity);
            Stationary = new UpdatedValue<bool>(stationary);
            Trajectory = new UpdatedValue<Vector2Double[]>();
        }

        public PhysicsBody Clone() {
            return new PhysicsBody(Mass.Val, Position.Val, Rotation.Val, Velocity.Val, Stationary.Val);
        }
    }
}
