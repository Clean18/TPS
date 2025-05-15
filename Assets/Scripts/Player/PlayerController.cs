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

	[SerializeField] private KeyCode aimKey = KeyCode.Mouse1;

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
	}

	void HandleMovement()
	{
		// ȸ�� ���� �� �¿� ȸ���� ���� ���� ��ȯ
		Vector3 camRotateDir = movement.SetAimRotation();

		float moveSpeed;
		// IsAiming ���¿� ���� �̵��ӵ� ����
		if (status.IsAiming.Value)
			moveSpeed = status.WalkSpeed;
		else
			moveSpeed = status.RunSpeed;

		Vector3 moveDir = movement.SetMove(moveSpeed);
		status.IsMoving.Value = (moveDir != Vector3.zero);

		// ��ü�� ȸ��
		Vector3 avatarDir;
		if (status.IsAiming.Value)
			avatarDir = camRotateDir;
		else
			avatarDir = moveDir;

		movement.SetAvatarRotation(avatarDir);
	}

	void HandleAiming()
	{
		status.IsAiming.Value = Input.GetKey(aimKey);
	}

	public void SubscribeEvents()
	{
		// SetActive �Լ��� ���
		// ��Ŭ������ IsAiming�� Value�� ���� ����� ��
		// Invoke�� Value�� �μ��� SetActive �Լ� ����
		// > ��Ŭ���� ���� true / false�� ��
		status.IsAiming.Subscribe(aimCamera.gameObject.SetActive);
		status.IsAiming.Subscribe(SetAimAnimation);
	}

	public void UnsubscribeEvents()
	{
		status.IsAiming.Unsubscribe(aimCamera.gameObject.SetActive);
		status.IsAiming.Unsubscribe(SetAimAnimation);
	}

	void SetAimAnimation(bool value)
	{
		anim.SetBool("IsAim", value);
	}
}
