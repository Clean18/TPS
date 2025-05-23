using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player = ScriptableObjectLearn.PlayerController;

public abstract class ItemData : ScriptableObject
{
	public string Name;
	[TextArea] public string Description;
	public Sprite Icon;
	public GameObject Prefab;

	public abstract void Use(Player controller);
}
