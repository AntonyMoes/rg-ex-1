using System.Linq;
using Math;
using Physics.Calculators;
using UnityEngine;
using Values;

namespace Physics {
    public class PhysicsBodyAdapter : MonoBehaviour {
        [SerializeField] private double _mass;
        [SerializeField] private Vector2 _velocity;
        [SerializeField] private float _velocityMagnitude;

        [SerializeField] private float _angularVelocity;

        // [SerializeField] private bool _stationary;
        [SerializeField] private LineRenderer _trajectoryRenderer;

        public PhysicsBody PhysicsBody { get; private set; }

        public void Setup(IBodySimplifiedCalculator simplifiedCalculator = null) {
            var massProxy = new ProxyValue<double>(() => _mass, val => _mass = val);
            var rotationProxy = new ProxyValue<float>(
                () => Utils.GameToPhysicsRotation(transform.rotation),
                val => transform.rotation = Utils.PhysicsToGameRotation(val));
            var angularVelocityProxy = new ProxyValue<float>(
                () => _angularVelocity,
                val => _angularVelocity = val);
            // var stationaryProxy = new ProxyValue<bool>(() => _stationary, val => _stationary = val);

            PhysicsBody = new PhysicsBody(massProxy, Utils.GameToPhysicsPosition(transform.position),
                (Vector2Double) _velocity, rotationProxy, angularVelocityProxy, simplifiedCalculator);
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
            transform.position = Utils.PhysicsToGamePosition(position, transform.position.z);
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

            var positions = trajectory.Select(p => Utils.PhysicsToGamePosition(p, transform.position.z)).ToArray();
            _trajectoryRenderer.positionCount = positions.Length;
            _trajectoryRenderer.SetPositions(positions);
        }
    }
}
