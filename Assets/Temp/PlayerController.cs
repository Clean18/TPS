using UnityEngine;

namespace Test
{
	/// <summary>
	/// 테스트용 이동 클래스
	/// </summary>
	public class PlayerController : MonoBehaviour
	{
		public PlayerMovement movement;
		public PlayerStatus status;

		void Update()
		{
			MoveTest();

			// IsAiming 변경용 테스트 코드
			status.IsAiming.Value = Input.GetKey(KeyCode.Mouse1); // 우클릭 누르면 변명
		}

		public void MoveTest()
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
		}
	}
}
