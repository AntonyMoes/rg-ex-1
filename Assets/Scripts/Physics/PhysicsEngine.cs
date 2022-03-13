using System.Collections.Generic;
using System.Linq;
using Math;
using Time;
using UnityEngine;
using Values;

namespace Physics {
    public class PhysicsEngine : IFrameProcessor {
        private readonly Dictionary<PhysicsBody, Vector2Double> _forces = new Dictionary<PhysicsBody, Vector2Double>();

        private readonly Value<double> _gravityConstant;
        private readonly List<PhysicsBody> _physicsBodies = new List<PhysicsBody>();
        private readonly Value<int> _trajectoryFrameCount;
        private readonly Value<float> _trajectoryFrameTime;

        public PhysicsEngine(Value<double> gravityConstant, Value<float> trajectoryFrameTime,
            Value<int> trajectoryFrameCount) {
            _gravityConstant = gravityConstant;
            _trajectoryFrameTime = trajectoryFrameTime;
            _trajectoryFrameCount = trajectoryFrameCount;
        }

        public void ProcessFrame(float frameTime) {
            ProcessPhysicsFrame(frameTime, _physicsBodies, _forces);
            CalculateTrajectories(_trajectoryFrameTime, _trajectoryFrameCount, _physicsBodies);
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

        private void ProcessPhysicsFrame(float frameTime, List<PhysicsBody> physicsBodies,
            Dictionary<PhysicsBody, Vector2Double> forcesCache) {
            foreach (var body in physicsBodies) {
                forcesCache[body] = Vector2Double.zero;
            }

            CalculateGravities(physicsBodies, forcesCache);
            ApplyTime(frameTime, physicsBodies, forcesCache);
        }

        private void CalculateGravities(List<PhysicsBody> physicsBodies,
            Dictionary<PhysicsBody, Vector2Double> forcesCache) {
            // TODO: use strategy

            for (var i = 0; i < physicsBodies.Count; i++) {
                for (var j = i + 1; j < physicsBodies.Count; j++) {
                    var body1 = physicsBodies[i];
                    var body2 = physicsBodies[j];

                    var force = CalculateGravity(body1, body2);
                    var body1To2Force = (body2.Position.Val - body1.Position).normalized * force;
                    forcesCache[body1] += body1To2Force;
                    forcesCache[body2] -= body1To2Force;
                }
            }
        }

        private void ApplyTime(float frameTime, List<PhysicsBody> physicsBodies,
            Dictionary<PhysicsBody, Vector2Double> forcesCache) {
            foreach (var body in physicsBodies) {
                if (body.Stationary) {
                    continue;
                }

                body.Velocity.Val += forcesCache[body] / body.Mass * frameTime;
                body.Position.Val += body.Velocity.Val * frameTime;
            }
        }

        private void CalculateTrajectories(float frameTime, int frames, List<PhysicsBody> physicsBodies) {
            // TODO: optimize and maybe call on frame updates instead of physics updates

            var copies = physicsBodies.Select(b => b.Clone()).ToList();
            var forcesCache = copies.ToDictionary(c => c, c => Vector2Double.zero);
            foreach (var copy in copies) {
                if (copy.Trajectory.Val == null || copy.Trajectory.Val.Length != frames) {
                    copy.Trajectory.Val = new Vector2Double[frames];
                }
            }

            for (var i = 0; i < frames; i++) {
                ProcessPhysicsFrame(frameTime, copies, forcesCache);
                foreach (var copy in copies) {
                    if (copy.Stationary) {
                        continue;
                    }

                    copy.Trajectory.Val[i] = copy.Position;
                }
            }

            for (var i = 0; i < physicsBodies.Count; i++) {
                physicsBodies[i].Trajectory.Val = copies[i].Trajectory.Val;
            }
        }

        private double CalculateGravity(PhysicsBody a, PhysicsBody b) {
            return _gravityConstant.Val * a.Mass * b.Mass /
                   System.Math.Pow((a.Position.Val - b.Position).magnitude, 2d);
        }
    }
}
