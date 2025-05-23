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
	[SerializeField] private GameObject fireParticle;

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
		RaycastHit hit;
		IDamagable target = RayShoot(out hit);

		if (!hit.Equals(default))
		{
			PlayFireEffect(hit.point, Quaternion.LookRotation(hit.normal));
		}

		if (target == null)
		{
			Debug.Log("target == null");
			return true;
		}
		
		target.TakeDamage(shootDamage, transform.parent.transform);

		// MonoBehaviour로 변환
		if (target is MonoBehaviour mb)
		{
			//GameObject gameObject = mb.gameObject;
			mb.targetTransform = transform.parent.transform;
		}

		return true;
	}

	void HandleCanShot()
	{
		if (canShoot) return;

		currentCount -= Time.deltaTime;
	}

	IDamagable RayShoot(out RaycastHit hitTarget)
	{
		Ray ray = new Ray(cam.transform.position, cam.transform.forward);
		RaycastHit hit;
		Debug.DrawRay(ray.origin, ray.direction * attackRange, Color.red, 1f);
		if (Physics.Raycast(ray, out hit, attackRange, targetLayer))
		{
			hitTarget = hit;
			if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Monster"))
			{
				return ReferenceRegistry.GetProvider(hit.collider.gameObject).GetAs<NormalMonster>();
			}

			//return NormalMonster.MonsterDic[hit.transform];
		}
		else
		{
			hitTarget = default;
		}
		return null;
	}

	void PlayFireEffect(Vector3 position, Quaternion rotation)
	{
		Instantiate(fireParticle, position, rotation);
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
