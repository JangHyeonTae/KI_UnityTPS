using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DesignPattern
{
    public class ObservableProperty<T>
    {
        [SerializeField] private T _value;
        public T Value
        {
            get => _value;
            set
            {
                if(_value.Equals(value)) return;
                _value = value;
                Notify();
            }
        }

        private UnityEvent<T> OnValueChanged = new();

        public ObservableProperty(T value = default)
        {
            _value = value;
        }
        public void Subscribe(UnityAction<T> action)
        {
            OnValueChanged.AddListener(action);
        }

        public void UnSubscribe(UnityAction<T> action)
        {
            OnValueChanged.RemoveListener(action);
        }

        private void UnSubscribeAll(UnityAction<T> action)
        {
            OnValueChanged.RemoveAllListeners();
        }

        private void Notify()
        {
            OnValueChanged?.Invoke(Value);
        }
    }
}

