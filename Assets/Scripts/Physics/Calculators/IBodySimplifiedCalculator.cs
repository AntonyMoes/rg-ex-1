using Math;

namespace Physics.Calculators {
    public interface IBodySimplifiedCalculator {
        public double Time { get; set; }
        public void CalculateProperties(double gravityConstant, out Vector2Double position, out float rotation);
    }
}
