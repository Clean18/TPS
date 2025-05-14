using UnityEngine;

namespace Test
{
	/// <summary>
	/// �׽�Ʈ�� �̵� Ŭ����
	/// </summary>
	public class PlayerController : MonoBehaviour
	{
		public PlayerMovement movement;
		public PlayerStatus status;

		void Update()
		{
			MoveTest();

			// IsAiming ����� �׽�Ʈ �ڵ�
			status.IsAiming.Value = Input.GetKey(KeyCode.Mouse1); // ��Ŭ�� ������ ����
		}

		public void MoveTest()
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
	}
}
