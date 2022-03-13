using System.Collections.Generic;
using System.Linq;
using Math;
using Physics.Calculators;
using Time;
using UnityEngine;
using Values;

namespace Physics {
    public class PhysicsEngine : IFrameProcessor {
        private readonly IPhysicsCalculator _calculator;
        private readonly Dictionary<PhysicsBody, Vector2Double> _forces = new Dictionary<PhysicsBody, Vector2Double>();

        private readonly Value<double> _gravityConstant;

        private readonly List<PhysicsBody> _physicsBodies = new List<PhysicsBody>();
        private readonly Value<int> _trajectoryFrameCount;
        private readonly Value<double> _trajectoryFrameTime;

        public PhysicsEngine(IPhysicsCalculator calculator, Value<double> gravityConstant,
            Value<double> trajectoryFrameTime,
            Value<int> trajectoryFrameCount) {
            _calculator = calculator;
            _gravityConstant = gravityConstant;
            _trajectoryFrameTime = trajectoryFrameTime;
            _trajectoryFrameCount = trajectoryFrameCount;
        }

        public void ProcessFrame(double frameTime) {
            ProcessPhysicsFrame(frameTime, _physicsBodies, _forces);
            // CalculateTrajectories(_trajectoryFrameTime, _trajectoryFrameCount, _physicsBodies);
        }

        public void RegisterBody(PhysicsBody body) {
            if (!_physicsBodies.Contains(body)) {
                _physicsBodies.Add(body);
                _forces.Add(body, Vector2.zero);
            }

            _calculator.RegisterBody(body);
        }

        public void UnregisterBody(PhysicsBody body) {
            _physicsBodies.Remove(body);
            _forces.Remove(body);

            _calculator.UnregisterBody(body);
        }

        private void ProcessPhysicsFrame(double frameTime, List<PhysicsBody> physicsBodies,
            Dictionary<PhysicsBody, Vector2Double> forcesCache) {
            foreach (var body in physicsBodies) {
                forcesCache[body] = Vector2Double.zero;
            }

            foreach (var (body, force) in _calculator.CalculateGravity(_gravityConstant)) {
                forcesCache[body] += force;
            }

            _calculator.AdvanceTime(_gravityConstant, frameTime, physicsBodies, forcesCache);
        }

        private void CalculateTrajectories(double frameTime, int frames, List<PhysicsBody> physicsBodies) {
            // TODO: optimize and maybe call on frame updates instead of physics updates
            return;

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
                    // if (copy.Stationary) {
                    //     continue;
                    // }

                    copy.Trajectory.Val[i] = copy.Position;
                }
            }

            for (var i = 0; i < physicsBodies.Count; i++) {
                physicsBodies[i].Trajectory.Val = copies[i].Trajectory.Val;
            }
        }
    }
}
