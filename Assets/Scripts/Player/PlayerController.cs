using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
	public bool IsControlActivate { get; set; } = true;

	[SerializeField] private PlayerStatus status;
	[SerializeField] private PlayerMovement movement;
	[SerializeField] private Animator anim;

	[SerializeField] private CinemachineVirtualCamera aimCamera;

	[SerializeField] private Gun gun;

	[SerializeField] private KeyCode aimKey = KeyCode.Mouse1;
	[SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

	void Awake()
	{
		Init();
	}

	void OnEnable()
	{
		SubscribeEvents();
	}

	void Update()
	{
		HandlePlayerControl();
	}

	void OnDisable()
	{
		UnsubscribeEvents();
	}

	//

	void Init()
	{
		status = GetComponent<PlayerStatus>();
		movement = GetComponent<PlayerMovement>();
		anim = GetComponent<Animator>();
	}

	void HandlePlayerControl()
	{
		if (!IsControlActivate)
			return;

		HandleMovement();
		HandleAiming();
		HandleShooting();
	}

	void HandleMovement()
	{
		// 회전 수행 후 좌우 회전에 대한 벡터 반환
		Vector3 camRotateDir = movement.SetAimRotation();

		float moveSpeed;
		// IsAiming 상태에 따라 이동속도 변경
		if (status.IsAiming.Value)
			moveSpeed = status.WalkSpeed;
		else
			moveSpeed = status.RunSpeed;

		Vector3 moveDir = movement.SetMove(moveSpeed);
		status.IsMoving.Value = (moveDir != Vector3.zero);

		// 몸체의 회전
		Vector3 avatarDir;
		if (status.IsAiming.Value)
			avatarDir = camRotateDir;
		else
			avatarDir = moveDir;

		movement.SetAvatarRotation(avatarDir);

		// 애니메이터 파라미터 추가
		if (status.IsAiming.Value)
		{
			Vector3 input = movement.GetInputDirection();
			anim.SetFloat("X", input.x);
			anim.SetFloat("Z", input.z);
		}
	}

	void HandleAiming()
	{
		status.IsAiming.Value = Input.GetKey(aimKey);
	}

	void HandleShooting()
	{
		if (status.IsAiming.Value && Input.GetKey(shootKey))
		{
			status.IsAttacking.Value = gun.Shoot();
		}
		else
		{
			status.IsAttacking.Value = false;
		}
	}

	public void SubscribeEvents()
	{
		// SetActive 함수를 등록
		// 우클릭으로 IsAiming의 Value의 값이 변경될 시
		// Invoke로 Value를 인수로 SetActive 함수 실행
		// > 우클릭에 따라 true / false가 됨
		status.IsAiming.Subscribe(aimCamera.gameObject.SetActive);
		status.IsAiming.Subscribe(SetAimAnimation);

		status.IsMoving.Subscribe(SetMoveAnimation);

		status.IsAttacking.Subscribe(SetAttackAnimation);
	}

	public void UnsubscribeEvents()
	{
		status.IsAiming.Unsubscribe(aimCamera.gameObject.SetActive);
		status.IsAiming.Unsubscribe(SetAimAnimation);

		status.IsMoving.Unsubscribe(SetMoveAnimation);

		status.IsAttacking.Unsubscribe(SetAttackAnimation);
	}

	void SetAimAnimation(bool value)
	{
		anim.SetBool("IsAim", value);
	}
	void SetMoveAnimation(bool value)
	{
		anim.SetBool("IsMove", value);
	}
	void SetAttackAnimation(bool value)
	{
		anim.SetBool("IsAttack", value);
	}
}
