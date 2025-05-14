using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

			// set���� ���� ����Ǹ� �˸� ����
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
