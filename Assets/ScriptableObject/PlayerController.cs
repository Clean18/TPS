using UnityEngine;

namespace ScriptableObjectLearn
{
	public class PlayerController : MonoBehaviour
	{
		[SerializeField] private int hp;

		[SerializeField] Inventory inventory;

		void Awake()
		{
			Init();
		}

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				inventory.UseItem(1);
			}
		}

		void OnTriggerEnter(Collider other)
		{
			Debug.Log("뭔가닿음");
			inventory.GetItem(other.GetComponent<ItemObject>().Data);
			other.gameObject.SetActive(false);
		}

		void OnCollisionEnter(Collision collision)
		{
			Debug.Log("뭔가닿음");
		}

		void Init()
		{
			inventory = GetComponent<Inventory>();
		}

		public void Recover(int value)
		{
			hp += value;
		}
	}
}
