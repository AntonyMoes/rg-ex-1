using System;

namespace Values {
    public class UpdatedValue<T> : Value<T> {
        private readonly Value<T> _value;
        private Action<T> _onValueUpdate;

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
                TriggerUpdate();
            }
        }

        public void AddUpdateListener(Action<T> listener) {
            _onValueUpdate += listener;
        }

        public void RemoveUpdateListener(Action<T> listener) {
            _onValueUpdate -= listener;
        }

        public void TriggerUpdate() {
            _onValueUpdate?.Invoke(_value.Val);
        }
    }
}
