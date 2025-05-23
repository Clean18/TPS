using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
	[field: SerializeField] public ItemData Data { get; private set; }

	GameObject childObject;

	void OnEnable()
	{
		childObject = Instantiate(Data.Prefab, transform);
	}

	void OnDisable()
	{
		Destroy(childObject);
	}
}
