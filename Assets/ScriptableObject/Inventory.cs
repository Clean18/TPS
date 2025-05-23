using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player = ScriptableObjectLearn.PlayerController;

public class Inventory : MonoBehaviour
{
	[SerializeField] List<ItemData> slots = new();
	[SerializeField] private Player controller;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		controller = GetComponent<Player>();
	}

	public void GetItem(ItemData itemData)
	{
		slots.Add(itemData);
	}

	public void UseItem(int index)
	{
		slots[index - 1].Use(controller);
		slots.RemoveAt(index - 1);
	}
}
