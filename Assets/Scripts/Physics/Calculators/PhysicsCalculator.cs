using System;
using System.Collections.Generic;
using System.Linq;
using Math;

namespace Physics.Calculators {
    public class PhysicsCalculator : IPhysicsCalculator {
        private readonly List<PhysicsBody> _bodies = new List<PhysicsBody>();
        private readonly Dictionary<PhysicsBody, TrajectoryData> _trajectories = new Dictionary<PhysicsBody, TrajectoryData>();

        private readonly Dictionary<PhysicsBody, Vector2Double> _gravityForces =
            new Dictionary<PhysicsBody, Vector2Double>();

        private double _trajectoryTimeStep;
        private double _timeToNextTrajectoryStep;

        public void RegisterBody(PhysicsBody body) {
            if (!_bodies.Contains(body)) {
                _bodies.Add(body);
                _gravityForces.Add(body, Vector2Double.zero);
            }
        }

        public void UnregisterBody(PhysicsBody body) {
            _bodies.Remove(body);
            _gravityForces.Remove(body);
        }

        public IReadOnlyDictionary<PhysicsBody, Vector2Double> CalculateGravity(double gravityConstant) {
            foreach (var body in _bodies) {
                _gravityForces[body] = Vector2Double.zero;
            }

            for (var i = 0; i < _bodies.Count; i++) {
                for (var j = i + 1; j < _bodies.Count; j++) {
                    var body1 = _bodies[i];
                    var body2 = _bodies[j];

                    var force = Utils.CalculateGravity(gravityConstant, body1.Mass, body1.Position,
                        body2.Mass, body2.Position);
                    var body1To2Force = (body2.Position.Val - body1.Position).normalized * force;
                    _gravityForces[body1] += body1To2Force;
                    _gravityForces[body2] -= body1To2Force;
                }
            }

            return _gravityForces;
        }

        public void AdvanceTime(double gravityConstant, double timeStep, IReadOnlyList<PhysicsBody> bodies,
            IReadOnlyDictionary<PhysicsBody, Vector2Double> forces) {
            foreach (var body in bodies) {
                body.Velocity.Val += forces[body] / body.Mass * timeStep;
                body.Position.Val += body.Velocity.Val * timeStep;
            }

            // TODO advance trajectories
        }

        // TODO maybe move this out to a shared parent???
        public void SetTrajectoryTimeStep(double timeStep) {
            _trajectoryTimeStep = timeStep;
            _timeToNextTrajectoryStep = 0d;
        }

        public void RegisterBodyForTrajectoryCalculation(PhysicsBody body, int steps) {
            if (!_trajectories.ContainsKey(body))
                _trajectories.Add(body, new TrajectoryData {
                    Trajectory = new Trajectory(steps),
                    Recalculate = true
                });
        }

        public void UnregisterBodyForTrajectoryCalculation(PhysicsBody body) {
            _trajectories.Remove(body);
        }

        public Vector2Double[] GetTrajectory(PhysicsBody body) {
            if (_trajectories.TryGetValue(body, out var trajectoryData))
                return trajectoryData.Trajectory.Path;

            return Array.Empty<Vector2Double>();
        }

        private struct TrajectoryData {
            public Trajectory Trajectory;
            public bool Recalculate;
        }
    }
}
