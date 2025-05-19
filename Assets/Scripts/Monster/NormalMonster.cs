using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NormalMonster : Monster, IDamagable
{
	[SerializeField] private bool isActivateControl = true;
	[SerializeField] private bool canTracking = true;
	private ObservableProperty<bool> IsMoving = new();
	private ObservableProperty<bool> IsAttacking = new();

	private ObservableProperty<int> CurrentHp = new();
	[SerializeField] private int MaxHp;

	[Header("Config NavMesh")]
	private NavMeshAgent navAgent;
	[SerializeField] private Transform targetTransform;

	// GetComponent를 사용하지 않고 IDamagable을 반환받기
	public static Dictionary<Transform, IDamagable> MonsterDic = new Dictionary<Transform, IDamagable>();

	void Awake()
	{
		Init();
	}

	void Update()
	{
		HandleControl();
	}

	void OnEnable()
	{
		// 활성화될 때 최대체력 유지
		CurrentHp.Value = MaxHp;
	}

	void Init()
	{
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.isStopped = true;

		MonsterDic.Add(transform, this);
	}

	void HandleControl()
	{
		if (!isActivateControl)
			return;

		HandleMove();
	}

	void HandleMove()
	{
		if (targetTransform == null)
		{
			return;
		}

		if (canTracking)
		{
			navAgent.SetDestination(targetTransform.position);
		}

		navAgent.isStopped = !canTracking;
		IsMoving.Value = canTracking;
	}

	public void TakeDamage(int value)
	{
		
	}

	public void TakeDamage(int value, Transform attacker)
	{
		// 대미지 판정
		// 체력감소
		CurrentHp.Value--;
		Debug.Log($"체력감소 : {CurrentHp.Value}");

		// 플레이어 추격
		targetTransform = attacker;
		Debug.Log($"추격 대상 변경 : {attacker.name}");

		// 체력 0 이하면 Dead
		if (CurrentHp.Value <= 0)
		{
			Debug.Log($"몬스터 사망 : ");
			gameObject.SetActive(false);
		}
	}
}
