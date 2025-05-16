using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpGuageUI : MonoBehaviour
{
	// 2. 현재 카메라의 방향으로 회전 즉 카메라의 방향 벡터 적용

	[SerializeField] private Image image;
	private Transform cameraTransform;

	void Awake()
	{
		Init();
	}

	void LateUpdate()
	{
		SetUIForwardVector(cameraTransform.forward);
	}

	void Init()
	{
		cameraTransform = Camera.main.transform;
	}

	public void SetImageFillAmount(float value)
	{
		// 현재 수치 / 최대 수치 = value
		image.fillAmount = value;
	}

	public void SetUIForwardVector(Vector3 target)
	{
		transform.forward = target;
	}
}
