using System;

namespace Values {
    public class ProxyValue<T> : Value<T> {
        private readonly Func<T> _getter;
        private readonly Action<T> _setter;

        public ProxyValue(Func<T> getter, Action<T> setter) {
            _getter = getter;
            _setter = setter;
        }

        public override T Val {
            get => _getter();
            set => _setter(value);
        }
    }
}
