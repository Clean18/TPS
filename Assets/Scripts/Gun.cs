using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
	[SerializeField] private LayerMask targetLayer;
	[SerializeField][Range(0, 100)] private float attackRange;
	[SerializeField] private int shootDamage;
	[SerializeField] private float shootDelay;
	[SerializeField] private AudioClip shootSFX;

	private CinemachineImpulseSource impulse;

	private Camera cam;

	private bool canShoot
	{
		get { return currentCount <= 0; }
		//set { canShoot = value; }
	}

	private float currentCount;

	void Awake()
	{
		Init();
	}

	void Update()
	{
		HandleCanShot();
	}

	void Init()
	{
		cam = Camera.main;
		impulse = GetComponent<CinemachineImpulseSource>();
	}

	public bool Shoot()
	{
		if (!canShoot)
			return false;

		PlayShootSound();
		PlayCameraEffect();
		PlayShootEffect();

		currentCount = shootDelay;

		// TODO : Ray 발사 > 반환받은 대상에게 대미지 입힘
		GameObject target = RayShoot();
		if (target == null)
			return true;

		Debug.Log($"총에 맞음 : {target.name}");

		return true;
	}

	void HandleCanShot()
	{
		if (canShoot) return;

		currentCount -= Time.deltaTime;
	}

	GameObject RayShoot()
	{
		Ray ray = new Ray(cam.transform.position, cam.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, attackRange, targetLayer))
		{
			return hit.transform.gameObject;
			// TODO : 몬스터를 어떻게 구현하는가에 따라 다름
		}
		return null;
	}

	void PlayShootSound()
	{
		SFXController sfx = GameManager.Instance.AudioManager.GetSFX();
		sfx.Play(shootSFX);
	}

	void PlayCameraEffect()
	{
		impulse.GenerateImpulse();
	}

	void PlayShootEffect()
	{
		// TODO : 총구 화염 이펙트 (파티클)
	}
}
