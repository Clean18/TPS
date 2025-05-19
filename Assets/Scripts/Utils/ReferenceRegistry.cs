using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public static class ReferenceRegistry
{
	private static Dictionary<GameObject, ReferenceProvider> providers { get; set; } = new();

	public static void Register(ReferenceProvider refPro)
	{
		if (providers.ContainsKey(refPro.gameObject))
			return;

		providers.Add(refPro.gameObject, refPro);
	}

	public static void Unregister(ReferenceProvider refPro)
	{
		if (!providers.ContainsKey(refPro.gameObject))
			return;

		providers.Remove(refPro.gameObject);
	}

	public static void Clear()
	{
		providers.Clear();
	}

	public static ReferenceProvider GetProvider(GameObject gameObject)
	{
		if (!providers.ContainsKey(gameObject))
			return null;

		return providers[gameObject];
	}
}
