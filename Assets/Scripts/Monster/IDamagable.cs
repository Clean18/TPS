using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
	public void TakeDamage(int value);
	public void TakeDamage(int value, Transform attacker);
}
