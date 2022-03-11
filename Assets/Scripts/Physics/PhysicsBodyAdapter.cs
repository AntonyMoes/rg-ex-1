using UnityEngine;
using Values;

namespace Physics {
    public class PhysicsBodyAdapter : MonoBehaviour {
        [SerializeField] private float _mass;
        [SerializeField] private Vector2 _velocity;
        [SerializeField] private float _velocityMagnitude;
        [SerializeField] private bool _stationary;

        public PhysicsBody PhysicsBody { get; private set; }

        public void Setup() {
            var massProxy = new ProxyValue<float>(() => _mass, val => _mass = val);
            var positionProxy = new ProxyValue<Vector2>(
                () => GameToPhysicsPosition(transform.position),
                val => transform.position = PhysicsToGamePosition(val, transform.position.z));
            var rotationProxy = new ProxyValue<float>(
                () => GameToPhysicsRotation(transform.rotation),
                val => transform.rotation = PhysicsToGameRotation(val));
            var velocityProxy = new ProxyValue<Vector2>(() => _velocity, val => _velocity = val);
            var stationaryProxy = new ProxyValue<bool>(() => _stationary, val => _stationary = val);

            PhysicsBody = new PhysicsBody(massProxy, positionProxy, rotationProxy, velocityProxy, stationaryProxy);
            PhysicsBody.Velocity.AddUpdateListener(OnVelocityUpdate);
        }

        public void Dispose() {
            PhysicsBody.Velocity.RemoveUpdateListener(OnVelocityUpdate);
            PhysicsBody = null;
        }

        private void OnVelocityUpdate(Vector2 velocity) {
            _velocityMagnitude = _velocity.magnitude;
        }

        private static Vector2 GameToPhysicsPosition(Vector3 position) {
            return position;
        }

        private static Vector3 PhysicsToGamePosition(Vector2 position, float z) {
            return (Vector3) position + Vector3.forward * z;
        }

        private static float GameToPhysicsRotation(Quaternion rotation) {
            return -rotation.eulerAngles.y;
        }

        private static Quaternion PhysicsToGameRotation(float rotation) {
            return Quaternion.Euler(0f, -rotation, 0f);
        }
    }
}
