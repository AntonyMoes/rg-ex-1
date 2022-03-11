using System.Collections.Generic;
using Time;
using UnityEngine;
using Values;

namespace Physics {
    public class PhysicsEngine : IFrameProcessor {
        private readonly Dictionary<PhysicsBody, Vector2> _forces = new Dictionary<PhysicsBody, Vector2>();
        private readonly List<PhysicsBody> _physicsBodies = new List<PhysicsBody>();

        public PhysicsEngine(Value<float> gravityConstant) {
            GravityConstant = gravityConstant;
        }

        public Value<float> GravityConstant { get; }

        public void ProcessFrame(float frameTime) {
            for (var i = 0; i < _physicsBodies.Count; i++) {
                for (var j = i + 1; j < _physicsBodies.Count; j++) {
                    var body1 = _physicsBodies[i];
                    var body2 = _physicsBodies[j];

                    var force = CalculateGravity(body1, body2);
                    var body1To2Force = (body2.Position.Val - body1.Position).normalized * force;
                    _forces[body1] += body1To2Force;
                    _forces[body2] -= body1To2Force;
                }
            }

            foreach (var body in _physicsBodies) {
                if (!body.Stationary) {
                    body.Velocity.Val += _forces[body] / body.Mass * frameTime;
                    body.Position.Val += body.Velocity.Val * frameTime;
                }

                _forces[body] = Vector2.zero;
            }
        }

        public void RegisterBody(PhysicsBody body) {
            if (!_physicsBodies.Contains(body)) {
                _physicsBodies.Add(body);
                _forces.Add(body, Vector2.zero);
            }
        }

        public void UnregisterBody(PhysicsBody body) {
            _physicsBodies.Remove(body);
            _forces.Remove(body);
        }

        private float CalculateGravity(PhysicsBody a, PhysicsBody b) {
            return GravityConstant.Val * a.Mass * b.Mass / Mathf.Pow((a.Position.Val - b.Position).magnitude, 2f);
        }
    }
}
