using System.Linq;
using Math;
using UnityEngine;
using Values;

namespace Physics {
    public class PhysicsBodyAdapter : MonoBehaviour {
        [SerializeField] private double _mass;
        [SerializeField] private Vector2 _velocity;
        [SerializeField] private float _velocityMagnitude;
        [SerializeField] private bool _stationary;
        [SerializeField] private LineRenderer _trajectoryRenderer;

        public PhysicsBody PhysicsBody { get; private set; }

        public void Setup() {
            var massProxy = new ProxyValue<double>(() => _mass, val => _mass = val);
            var rotationProxy = new ProxyValue<float>(
                () => GameToPhysicsRotation(transform.rotation),
                val => transform.rotation = PhysicsToGameRotation(val));
            var stationaryProxy = new ProxyValue<bool>(() => _stationary, val => _stationary = val);

            PhysicsBody = new PhysicsBody(massProxy, GameToPhysicsPosition(transform.position),
                rotationProxy, (Vector2Double) _velocity, stationaryProxy);
            PhysicsBody.Position.AddUpdateListener(OnPositionUpdate);
            PhysicsBody.Velocity.AddUpdateListener(OnVelocityUpdate);
            PhysicsBody.Trajectory.AddUpdateListener(OnTrajectoryUpdate);
        }

        public void Dispose() {
            PhysicsBody.Position.RemoveUpdateListener(OnPositionUpdate);
            PhysicsBody.Velocity.RemoveUpdateListener(OnVelocityUpdate);
            PhysicsBody.Trajectory.RemoveUpdateListener(OnTrajectoryUpdate);
            PhysicsBody = null;
        }

        private void OnPositionUpdate(Vector2Double position) {
            transform.position = PhysicsToGamePosition(position, transform.position.z);
        }

        private void OnVelocityUpdate(Vector2Double velocity) {
            _velocity = velocity;
            _velocityMagnitude = _velocity.magnitude;
        }

        private void OnTrajectoryUpdate(Vector2Double[] trajectory) {
            if (_trajectoryRenderer == null) {
                return;
            }

            if (trajectory == null) {
                _trajectoryRenderer.SetPositions(new Vector3[] { });
                return;
            }

            var positions = trajectory.Select(p => PhysicsToGamePosition(p, transform.position.z)).ToArray();
            _trajectoryRenderer.positionCount = positions.Length;
            _trajectoryRenderer.SetPositions(positions);
        }

        private static Vector2Double GameToPhysicsPosition(Vector3 position) {
            return (Vector2) position;
        }

        private static Vector3 PhysicsToGamePosition(Vector2Double position, float z) {
            return (Vector3) (Vector2) position + Vector3.forward * z;
        }

        private static float GameToPhysicsRotation(Quaternion rotation) {
            return -rotation.eulerAngles.y;
        }

        private static Quaternion PhysicsToGameRotation(float rotation) {
            return Quaternion.Euler(0f, -rotation, 0f);
        }
    }
}
