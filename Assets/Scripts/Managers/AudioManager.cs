using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource bgmSource;
	private ObjectPool sfxPool;

	[SerializeField] private List<AudioClip> bgmList = new();
	[SerializeField] private SFXController sfxPrefab;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		bgmSource = GetComponent<AudioSource>();

		sfxPool = new ObjectPool(sfxPrefab, transform, 10);
	}

	public void BgmPlay(int index)
	{
		if (0 <= index && index < bgmList.Count)
		{
			bgmSource.Stop();
			bgmSource.clip = bgmList[index];
			bgmSource.Play();
		}
	}

	public SFXController GetSFX()
	{
		// 풀에서 꺼내와서
		PooledObject po = sfxPool.GetPool();
		// SFXController 타입으로 변환하면서 반환
		return po as SFXController;
	}
}
