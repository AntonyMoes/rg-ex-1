using System;

namespace Values {
    public class UpdatedValue<T> : Value<T> {
        private readonly Value<T> _value;
        private Action<T> _onValueSet;

        public UpdatedValue(T initialValue = default) {
            _value = initialValue;
        }

        public UpdatedValue(Value<T> initialValue) {
            _value = initialValue;
        }

        public override T Val {
            get => _value.Val;
            set {
                if (Equals(_value, value)) {
                    return;
                }

                _value.Val = value;
                _onValueSet?.Invoke(_value.Val);
            }
        }

        public void AddUpdateListener(Action<T> listener) {
            _onValueSet += listener;
        }

        public void RemoveUpdateListener(Action<T> listener) {
            _onValueSet -= listener;
        }

        public static implicit operator T(UpdatedValue<T> value) {
            return value.Val;
        }
    }
}
