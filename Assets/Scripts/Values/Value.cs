﻿namespace Values {
    public class Value<T> {
        public virtual T Val { get; set; }

        public static implicit operator Value<T>(T value) {
            return new Value<T> {Val = value};
        }
    }
}
