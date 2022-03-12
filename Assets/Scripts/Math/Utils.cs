namespace Math {
    public static class Utils {
        public static double Clamp(this double value, double min, double max) {
            return System.Math.Min(System.Math.Max(value, min), max);
        }
    }
}
