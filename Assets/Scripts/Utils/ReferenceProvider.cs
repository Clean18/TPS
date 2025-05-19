using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class ReferenceProvider : MonoBehaviour
{
	[SerializeField] private Component component;

	void Awake()
	{
		ReferenceRegistry.Register(this);
	}

	void OnDestroy()
	{
		ReferenceRegistry.Unregister(this);
	}

	public T GetAs<T>() where T : Component
	{
		return component as T;
	}
}
