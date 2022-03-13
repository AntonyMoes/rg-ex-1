namespace Physics {
    public interface IForceApplier {
        public void AddForce(PhysicsBody body, Force force);
        public void RemoveForce(PhysicsBody body, Force force);
        public void AddForceImmediate(PhysicsBody body, Force force);
    }
}
