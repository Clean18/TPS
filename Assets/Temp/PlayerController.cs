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
			// 회전 수행 후 좌우 회전에 대한 벡터 반환
			Vector3 camRotateDir = movement.SetAimRotation();

			float moveSpeed;
			if (status.IsAiming.Value)
				moveSpeed = status.WalkSpeed;
			else
				moveSpeed = status.RunSpeed;

			Vector3 moveDir = movement.SetMove(moveSpeed);
			status.IsMoving.Value = (moveDir != Vector3.zero);

			// 몸체의 회전

		}
	}
}
