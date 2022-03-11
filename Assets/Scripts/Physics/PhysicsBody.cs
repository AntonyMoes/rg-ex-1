using UnityEngine;
using Values;

namespace Physics {
    public class PhysicsBody {
        public readonly UpdatedValue<float> Mass;
        public readonly UpdatedValue<Vector2> Position;
        public readonly UpdatedValue<float> Rotation;
        public readonly UpdatedValue<bool> Stationary;
        public readonly UpdatedValue<Vector2> Velocity;

        public PhysicsBody(Value<float> mass, Value<Vector2> position, Value<float> rotation, Value<Vector2> velocity,
            Value<bool> stationary) {
            Mass = new UpdatedValue<float>(mass);
            Position = new UpdatedValue<Vector2>(position);
            Rotation = new UpdatedValue<float>(rotation);
            Velocity = new UpdatedValue<Vector2>(velocity);
            Stationary = new UpdatedValue<bool>(stationary);
        }
    }
}
