using System.Collections.Generic;
using Math;

namespace Physics.Calculators {
    public interface IPhysicsCalculator {
        public void RegisterBody(PhysicsBody body);
        public void UnregisterBody(PhysicsBody body);

        public IReadOnlyDictionary<PhysicsBody, Vector2Double> CalculateGravity(double gravityConstant);

        public void AdvanceTime(double gravityConstant, double timeStep, IReadOnlyList<PhysicsBody> bodies,
            IReadOnlyDictionary<PhysicsBody, Vector2Double> forces);
    }
}
