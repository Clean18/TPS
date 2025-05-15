using DesignPattern;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXController : PooledObject
{
	[SerializeField] private AudioSource audioSource;
	[SerializeField] private float currentCount;

	void Awake()
	{
		Init();
	}

	void Init()
	{
		audioSource = GetComponent<AudioSource>();

	}

	void Update()
	{
		currentCount -= Time.deltaTime;

		// 재생 시간이 끝나면 오브젝트 풀로 리턴
		if (currentCount <= 0)
		{
			ReturnPool();
		}
	}

	public void Play(AudioClip clip, bool loop = false, bool onAwake = false)
	{
		// 오디오 클립을 재생
		audioSource.Stop();
		audioSource.clip = clip;
		audioSource.Play();
		audioSource.loop = loop;
		audioSource.playOnAwake = onAwake;

		// 클립의 재생 시간을 변수에 할당
		currentCount = clip.length;
	}
}
