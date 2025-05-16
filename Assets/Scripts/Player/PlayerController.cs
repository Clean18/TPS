using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamagable
{
	public bool IsControlActivate { get; set; } = true;

	[SerializeField] private PlayerStatus status;
	[SerializeField] private PlayerMovement movement;
	private Animator anim;
	[SerializeField] private Animator aimAnim;
	private Image aimImage;

	[SerializeField] private HpGuageUI hpUI;

	[SerializeField] private CinemachineVirtualCamera aimCamera;

	[SerializeField] private Gun gun;

	[SerializeField] private KeyCode aimKey = KeyCode.Mouse1;
	[SerializeField] private KeyCode shootKey = KeyCode.Mouse0;

	void Awake()
	{
		Init();
	}

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked; // 마우스를 화면 중앙에 고정
		Cursor.visible = false;                   // 마우스 커서를 숨김
	}

	void OnEnable()
	{
		SubscribeEvents();
	}

	void Update()
	{
		HandlePlayerControl();
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}

		if (Input.GetKey(KeyCode.Alpha1))
		{
			TakeDamage(1);
		}
		if (Input.GetKey(KeyCode.Alpha2))
		{
			RecoveryHp(1);
		}
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
		aimImage = aimAnim.GetComponent<Image>();

		hpUI.SetImageFillAmount(1);
		status.CurrentHp.Value = status.MaxHp;
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

	public void TakeDamage(int damage)
	{
		// 체력이 0이하가 되면 플레이어 사망 처리
		status.CurrentHp.Value -= damage;

		if (status.CurrentHp.Value <= 0)
		{
			Dead();
		}
	}

	public void RecoveryHp(int healAmount)
	{
		// MaxHp을 초과하지 않도록 회복
		int hp = status.CurrentHp.Value + healAmount;

		status.CurrentHp.Value = Mathf.Clamp(hp, 0, status.MaxHp);
	}

	public void Dead()
	{
		// TODO : 플레이어 사망 기능
		Debug.Log("플레이어 사망 처리");
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
		status.CurrentHp.Subscribe(SetHpUIGuage);

		status.IsAiming.Subscribe(aimCamera.gameObject.SetActive);
		status.IsAiming.Subscribe(SetAimAnimation);

		status.IsMoving.Subscribe(SetMoveAnimation);

		status.IsAttacking.Subscribe(SetAttackAnimation);
	}

	public void UnsubscribeEvents()
	{
		status.CurrentHp.Unsubscribe(SetHpUIGuage);

		status.IsAiming.Unsubscribe(aimCamera.gameObject.SetActive);
		status.IsAiming.Unsubscribe(SetAimAnimation);

		status.IsMoving.Unsubscribe(SetMoveAnimation);

		status.IsAttacking.Unsubscribe(SetAttackAnimation);
	}

	void SetAimAnimation(bool value)
	{
		// 에임 애니메이션의 디폴트는 1 > 0 이라 최초 런타임에서 에임이 사라지는 애니메이션이 재생되서 컴포넌트를 꺼두고 조건문 사용
		if (!aimImage.enabled)
		{
			aimImage.enabled = true;
		}
		anim.SetBool("IsAim", value);
		aimAnim.SetBool("IsAim", value);
	}
	void SetMoveAnimation(bool value)
	{
		anim.SetBool("IsMove", value);
	}
	void SetAttackAnimation(bool value)
	{
		anim.SetBool("IsAttack", value);
	}

	void SetHpUIGuage(int currentHp)
	{
		// 현재 수치 / 최대 수치
		float hpPer = (float)currentHp / status.MaxHp;
		hpUI.SetImageFillAmount(hpPer);
	}
}
