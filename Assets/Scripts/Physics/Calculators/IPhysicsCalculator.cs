using System.Collections.Generic;
using Math;

namespace Physics.Calculators {
    public interface IPhysicsCalculator {
        public void RegisterBody(PhysicsBody body);
        public void UnregisterBody(PhysicsBody body);

        public IReadOnlyDictionary<PhysicsBody, Vector2Double> CalculateGravity(double gravityConstant);

        public void AdvanceTime(double gravityConstant, double timeStep, IReadOnlyList<PhysicsBody> bodies,
            IReadOnlyDictionary<PhysicsBody, Vector2Double> forces);

        public void RegisterBodyForTrajectoryCalculation(PhysicsBody body, double trajectoryTimeStep, int steps);
        public void UnregisterBodyForTrajectoryCalculation(PhysicsBody body);

        // TODO: trajectories are calculated ahead of time by an amount of steps and are updated once the first step expires
        // maybe create Trajectory class for this
        public IReadOnlyDictionary<PhysicsBody, Vector2Double[]> GetTrajectories();
    }
}
