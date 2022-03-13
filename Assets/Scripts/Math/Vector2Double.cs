using System;
using System.Globalization;
using UnityEngine;

namespace Math {
    public struct Vector2Double {
        public double x;
        public double y;

        public Vector2Double(double x, double y) {
            this.x = x;
            this.y = y;
        }

        public static Vector2Double Scale(Vector2Double a, Vector2Double b) {
            return new Vector2Double(a.x * b.x, a.y * b.y);
        }

        public void Scale(Vector2Double scale) {
            x *= scale.x;
            y *= scale.y;
        }

        public void Normalize() {
            var magnitude = this.magnitude;
            if (magnitude > 9.99999974737875E-06) {
                this = this / magnitude;
            } else {
                this = zero;
            }
        }

        public Vector2Double normalized {
            get {
                var vec = new Vector2Double(x, y);
                vec.Normalize();
                return vec;
            }
        }

        public override string ToString() {
            return ToString(null, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format) {
            return ToString(format, CultureInfo.InvariantCulture.NumberFormat);
        }

        public string ToString(string format, IFormatProvider formatProvider) {
            if (string.IsNullOrEmpty(format)) {
                format = "F1";
            }

            return
                $"({x.ToString(format, formatProvider)}, {y.ToString(format, formatProvider)})";
        }

        public override int GetHashCode() {
            return x.GetHashCode() ^ (y.GetHashCode() << 2);
        }

        public override bool Equals(object obj) {
            return obj is Vector2Double other && Equals(other);
        }

        public bool Equals(Vector2Double other) {
            return this == other;
        }

        public static Vector2Double Reflect(Vector2Double inDirection, Vector2Double inNormal) {
            var num = -2f * Dot(inNormal, inDirection);
            return new Vector2Double(num * inNormal.x + inDirection.x, num * inNormal.y + inDirection.y);
        }

        public static Vector2Double Perpendicular(Vector2Double inDirection) {
            return new Vector2Double(-inDirection.y, inDirection.x);
        }

        public static double Dot(Vector2Double lhs, Vector2Double rhs) {
            return lhs.x * rhs.x + lhs.y * rhs.y;
        }

        public double magnitude => System.Math.Sqrt(sqrMagnitude);

        public double sqrMagnitude => x * x + y * y;

        public static double Angle(Vector2Double from, Vector2Double to) {
            var num = System.Math.Sqrt(from.sqrMagnitude * to.sqrMagnitude);
            return num < 1.00000000362749E-15
                ? 0.0f
                : System.Math.Acos((Dot(from, to) / num).Clamp(-1d, 1d)) * Utils.Rad2Deg;
        }

        public static double SignedAngle(Vector2Double from, Vector2Double to) {
            return Angle(from, to) * - /*need minus for clockwise*/System.Math.Sign(from.x * to.y - from.y * to.x);
        }

        public static double Distance(Vector2Double a, Vector2Double b) {
            var num1 = a.x - b.x;
            var num2 = a.y - b.y;
            return System.Math.Sqrt(num1 * num1 + num2 * num2);
        }

        public static Vector2Double Rotate(Vector2Double vec, double degrees) {
            var sin = System.Math.Sin(degrees * Utils.Deg2Rad);
            var cos = System.Math.Cos(degrees * Utils.Deg2Rad);

            var newX = cos * vec.x + sin * vec.y;
            var newY = cos * vec.y - sin * vec.x;
            return new Vector2Double(newX, newY);
        }

        public static Vector2Double operator +(Vector2Double a, Vector2Double b) {
            return new Vector2Double(a.x + b.x, a.y + b.y);
        }

        public static Vector2Double operator -(Vector2Double a, Vector2Double b) {
            return new Vector2Double(a.x - b.x, a.y - b.y);
        }

        public static Vector2Double operator *(Vector2Double a, Vector2Double b) {
            return new Vector2Double(a.x * b.x, a.y * b.y);
        }

        public static Vector2Double operator /(Vector2Double a, Vector2Double b) {
            return new Vector2Double(a.x / b.x, a.y / b.y);
        }

        public static Vector2Double operator -(Vector2Double a) {
            return new Vector2Double(-a.x, -a.y);
        }

        public static Vector2Double operator *(Vector2Double a, double d) {
            return new Vector2Double(a.x * d, a.y * d);
        }

        public static Vector2Double operator *(double d, Vector2Double a) {
            return new Vector2Double(a.x * d, a.y * d);
        }

        public static Vector2Double operator /(Vector2Double a, double d) {
            return new Vector2Double(a.x / d, a.y / d);
        }

        public static bool operator ==(Vector2Double lhs, Vector2Double rhs) {
            var num1 = lhs.x - rhs.x;
            var num2 = lhs.y - rhs.y;
            return num1 * num1 + num2 * num2 < 9.99999943962493E-11;
        }

        public static bool operator !=(Vector2Double lhs, Vector2Double rhs) {
            return !(lhs == rhs);
        }

        public static implicit operator Vector2Double(Vector2 v) {
            return new Vector2Double(v.x, v.y);
        }

        public static implicit operator Vector2(Vector2Double v) {
            return new Vector2(Convert.ToSingle(v.x), Convert.ToSingle(v.y));
        }

        public static Vector2Double zero { get; } = new Vector2Double(0.0d, 0.0d);

        public static Vector2Double one { get; } = new Vector2Double(1d, 1d);

        public static Vector2Double up { get; } = new Vector2Double(0.0d, 1d);

        public static Vector2Double down { get; } = new Vector2Double(0.0d, -1d);

        public static Vector2Double left { get; } = new Vector2Double(-1d, 0.0d);

        public static Vector2Double right { get; } = new Vector2Double(1d, 0.0d);
    }
}
