using UnityEngine;

namespace DesignPattern
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		static T instance;

		public static T Instance
		{
			get
			{
				if (instance == null)
				{
					instance = FindObjectOfType<T>();
					DontDestroyOnLoad(instance);
				}
				return instance;
			}
		}

		protected void SingletonInit()
		{
			if (instance != null && instance != this)
			{
				Destroy(gameObject);
			}
			else
			{
				instance = this as T;
				DontDestroyOnLoad(instance);
			}
		}
	}
}
