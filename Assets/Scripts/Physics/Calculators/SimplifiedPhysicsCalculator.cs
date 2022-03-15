using System;
using System.Collections.Generic;
using Math;

namespace Physics.Calculators {
    public class SimplifiedPhysicsCalculator : IPhysicsCalculator {
        private readonly List<PhysicsBody> _gravityAppliers = new List<PhysicsBody>();

        private readonly Dictionary<PhysicsBody, Vector2Double> _gravityForces =
            new Dictionary<PhysicsBody, Vector2Double>();

        private readonly List<PhysicsBody> _gravityReceivers = new List<PhysicsBody>();

        private readonly Dictionary<PhysicsBody, Action<double>> _massChangeListeners =
            new Dictionary<PhysicsBody, Action<double>>();

        private readonly double _massToAffectGravity;

        public SimplifiedPhysicsCalculator(double massToAffectGravity) {
            _massToAffectGravity = massToAffectGravity;
        }

        public void RegisterBody(PhysicsBody body) {
            var shouldReceiveGravity = body.SimplifiedCalculator == null;
            if (shouldReceiveGravity && !_gravityReceivers.Contains(body)) {
                _gravityReceivers.Add(body);
                _gravityForces.Add(body, Vector2Double.zero);
            }

            var shouldApplyGravity = body.Mass >= _massToAffectGravity;
            if (shouldApplyGravity && !_gravityAppliers.Contains(body)) {
                _gravityAppliers.Add(body);
            }

            body.Mass.AddUpdateListener(OnMassUpdate);
            _massChangeListeners.Add(body, OnMassUpdate);

            void OnMassUpdate(double _) {
                OnBodyMassUpdate(body);
            }
        }

        public void UnregisterBody(PhysicsBody body) {
            _gravityReceivers.Remove(body);
            _gravityAppliers.Remove(body);
            _gravityForces.Remove(body);

            body.Mass.RemoveUpdateListener(_massChangeListeners[body]);
            _massChangeListeners.Remove(body);
        }

        public IReadOnlyDictionary<PhysicsBody, Vector2Double> CalculateGravity(double gravityConstant) {
            foreach (var body in _gravityReceivers) {
                _gravityForces[body] = Vector2Double.zero;
            }

            foreach (var receiver in _gravityReceivers) {
                foreach (var applier in _gravityAppliers) {
                    if (applier == receiver) {
                        continue;
                    }

                    var force = Utils.CalculateGravity(gravityConstant, receiver.Mass, receiver.Position,
                        applier.Mass, applier.Position);
                    var receiverToApplier = (applier.Position.Val - receiver.Position).normalized * force;
                    _gravityForces[receiver] += receiverToApplier;
                }
            }

            return _gravityForces;
        }

        public void AdvanceTime(double gravityConstant, double timeStep, IReadOnlyList<PhysicsBody> bodies,
            IReadOnlyDictionary<PhysicsBody, Vector2Double> forces) {
            foreach (var body in bodies) {
                if (body.SimplifiedCalculator != null) {
                    body.SimplifiedCalculator.Time += timeStep;
                    body.SimplifiedCalculator.CalculateProperties(gravityConstant, out var position, out var rotation);
                    body.Position.Val = position;
                    body.Rotation.Val = rotation;
                    continue;
                }

                body.Velocity.Val += forces[body] / body.Mass * timeStep;
                body.Position.Val += body.Velocity.Val * timeStep;
            }
        }

        public void SetTrajectoryTimeStep(double timeStep) {
            throw new NotImplementedException();
        }

        public void RegisterBodyForTrajectoryCalculation(PhysicsBody body, int steps) {
            throw new NotImplementedException();
        }

        public void UnregisterBodyForTrajectoryCalculation(PhysicsBody body) {
            throw new NotImplementedException();
        }

        public Vector2Double[] GetTrajectory(PhysicsBody body) {
            throw new NotImplementedException();
        }

        private void OnBodyMassUpdate(PhysicsBody body) {
            UnregisterBody(body);
            RegisterBody(body);
        }
    }
}
