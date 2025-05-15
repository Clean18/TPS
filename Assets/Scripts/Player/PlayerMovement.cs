using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[Header("References")]
	[SerializeField] private Transform avatar;
	[SerializeField] private Transform aim;

	private Rigidbody rigid;
	private PlayerStatus playerStatus;

	[Header("Mouse Config")]
	[SerializeField][Range(-90, 0)] private float minPitch;
	[SerializeField][Range(0, 90)] private float maxPitch;
	[SerializeField][Range(0, 5)] private float mouseSensitivity = 1;

	private Vector2 currentRotation;
	void Awake()
	{
		Init();
	}

	void Init()
	{
		rigid = GetComponent<Rigidbody>();
		playerStatus = GetComponent<PlayerStatus>();
	}

	public Vector3 SetMove(float moveSpeed)
	{
		Vector3 moveDirection = GetMoveDirection();

		Vector3 velocity = rigid.velocity;
		velocity.x = moveDirection.x * moveSpeed;
		velocity.z = moveDirection.z * moveSpeed;

		rigid.velocity = velocity;

		return moveDirection;
	}

	public Vector3 SetAimRotation()
	{
		Vector2 mouseDir = GetMouserDirection();

		// X���� ������ �� �ʿ䰡 ����
		currentRotation.x += mouseDir.x;

		// Y���� ������ �ɾ�� ��
		currentRotation.y = Mathf.Clamp(currentRotation.y + mouseDir.y, minPitch, maxPitch);

		// ĳ���� ������Ʈ�� ��� Y�� ȸ���� �ݿ��Ǿ���
		transform.rotation = Quaternion.Euler(0, currentRotation.x, 0);

		// ������ ��� ���� ȸ�� �ݿ�
		Vector3 currentEuler = aim.localEulerAngles;
		aim.localEulerAngles = new Vector3(currentRotation.y, currentEuler.y, currentEuler.z);

		// ȸ�� ���� ���� ��ȯ
		Vector3 rotateDirVector = transform.forward;
		rotateDirVector.y = 0;
		return rotateDirVector.normalized;
	}

	public void SetAvatarRotation(Vector3 direction)
	{
		if (direction == Vector3.zero)
			return;

		Quaternion targetRotation = Quaternion.LookRotation(direction);

		avatar.rotation = Quaternion.Lerp(avatar.rotation, targetRotation, playerStatus.RotateSpeed * Time.deltaTime);
	}

	public Vector3 GetMoveDirection()
	{
		Vector3 input = GetInputDirection();

		Vector3 dir = (transform.right * input.x) + (transform.forward * input.z);

		return dir.normalized;
	}

	public Vector3 GetInputDirection()
	{
		float x = Input.GetAxisRaw("Horizontal");
		float z = Input.GetAxisRaw("Vertical");
		return new Vector3(x, 0, z);
	}

	private Vector2 GetMouserDirection()
	{
		float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
		float mouseY = -Input.GetAxis("Mouse Y") * mouseSensitivity; // Y�� ���Ϲ���
		return new Vector2(mouseX, mouseY);
	}
}
