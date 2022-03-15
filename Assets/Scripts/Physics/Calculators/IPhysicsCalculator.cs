using System.Collections.Generic;
using Math;

namespace Physics.Calculators {
    public interface IPhysicsCalculator {
        public void RegisterBody(PhysicsBody body);
        public void UnregisterBody(PhysicsBody body);

        public IReadOnlyDictionary<PhysicsBody, Vector2Double> CalculateGravity(double gravityConstant);

        public void AdvanceTime(double gravityConstant, double timeStep, IReadOnlyList<PhysicsBody> bodies,
            IReadOnlyDictionary<PhysicsBody, Vector2Double> forces);

        public void SetTrajectoryTimeStep(double timeStep);
        public void RegisterBodyForTrajectoryCalculation(PhysicsBody body, int steps);
        public void UnregisterBodyForTrajectoryCalculation(PhysicsBody body);

        public Vector2Double[] GetTrajectory(PhysicsBody body);
    }
}
