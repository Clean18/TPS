using UnityEngine;

namespace Test
{
	public class PlayerController : MonoBehaviour
	{
		public PlayerMovement movement;
		public PlayerStatus status;

		void Update()
		{
			MoveTest();
		}

		public void MoveTest()
		{
			// ȸ�� ���� �� �¿� ȸ���� ���� ���� ��ȯ
			Vector3 camRotateDir = movement.SetAimRotation();

			float moveSpeed;
			if (status.IsAiming.Value)
				moveSpeed = status.WalkSpeed;
			else
				moveSpeed = status.RunSpeed;

			Vector3 moveDir = movement.SetMove(moveSpeed);
			status.IsMoving.Value = (moveDir != Vector3.zero);

			// ��ü�� ȸ��

		}
	}
}
