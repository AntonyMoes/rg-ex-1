namespace Math {
    public static class Utils {
        public const double Deg2Rad = 0.01745329d;
        public const double Rad2Deg = 57.29578d;

        public static double Clamp(this double value, double min, double max) {
            return System.Math.Min(System.Math.Max(value, min), max);
        }
    }
}
