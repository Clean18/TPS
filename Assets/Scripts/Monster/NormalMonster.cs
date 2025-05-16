using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPattern;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class NormalMonster : Monster, IDamagable
{
	private bool isActivateControl = true;
	private bool canTracking = true;
	private ObservableProperty<bool> IsMoving = new();
	private ObservableProperty<bool> IsAttacking = new();

	private ObservableProperty<int> CurrentHp = new();
	[SerializeField] private int MaxHp;

	[Header("Config NavMesh")]
	private NavMeshAgent navAgent;
	[SerializeField] private Transform targetTransform;

	void Awake()
	{
		Init();
	}

	void Update()
	{
		HandleControl();
	}

	void Init()
	{
		navAgent = GetComponent<NavMeshAgent>();
		navAgent.isStopped = true;
	}

	void HandleControl()
	{
		if (!isActivateControl)
			return;

		HandleMove();
	}

	void HandleMove()
	{
		if (IsMoving.Value || targetTransform == null)
			return;

		if (canTracking)
		{
			navAgent.SetDestination(targetTransform.position);
		}

		navAgent.isStopped = !canTracking;
		IsMoving.Value = canTracking;
	}

	public void TakeDamage(int value)
	{
		// 대미지 판정
		// 체력감소
		// 체력 0 이하면 Dead
	}
}
