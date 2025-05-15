using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;

public class GameManager : Singleton<GameManager>
{
	public AudioManager AudioManager { get; private set; }

    void Awake()
    {
        Init();
    }

    void Init()
    {
        base.SingletonInit();
		AudioManager = GetComponentInChildren<AudioManager>();
	}
}
