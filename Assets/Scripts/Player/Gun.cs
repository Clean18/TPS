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
		IDamagable target = RayShoot();
		if (target == null)
			return true;

		target.TakeDamage(shootDamage);

		return true;
	}

	void HandleCanShot()
	{
		if (canShoot) return;

		currentCount -= Time.deltaTime;
	}

	IDamagable RayShoot()
	{
		Ray ray = new Ray(cam.transform.position, cam.transform.forward);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, attackRange, targetLayer))
		{
			// 공격시마다 GetComponent를 받아오는데 이걸 최소화하는 방법?
			return hit.transform.gameObject.GetComponent<IDamagable>();
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
