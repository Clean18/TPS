using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player = ScriptableObjectLearn.PlayerController;

[CreateAssetMenu(fileName = "Hp Potion", menuName = "Scriptable Object/Hp Potion", order = 1)]
public class HpPotion : ItemData
{
	public int Value;

	public override void Use(Player controller)
	{
		controller.Recover(Value);
	}
}
