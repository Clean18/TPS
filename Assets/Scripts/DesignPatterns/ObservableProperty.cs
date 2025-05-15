using System;
using UnityEngine;
using UnityEngine.Events;

namespace DesignPattern
{
	public class ObservableProperty<T>
	{
		[SerializeField] private T _value;
		public T Value
		{
			get { return _value; }
			set
			{
				if (_value.Equals(value))
					return;

				// set에서 값이 변경되면 알림 전송
				_value = value;
				Notify();
			}
		}

		private UnityEvent<T> onValueChanged = new();

		public ObservableProperty(T value = default)
		{
			_value = value;
		}

		public void Subscribe(UnityAction<T> action)
		{
			onValueChanged.AddListener(action);
		}

		public void Unsubscribe(UnityAction<T> action)
		{
			onValueChanged.RemoveListener(action);
		}

		public void UnsubscribeAll()
		{
			onValueChanged.RemoveAllListeners();
		}

		private void Notify()
		{
			onValueChanged?.Invoke(Value);
		}
	}
}
