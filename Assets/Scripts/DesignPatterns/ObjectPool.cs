using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
	public class ObjectPool
	{
		private Stack<PooledObject> stack;	// 생성할 스택
		private PooledObject targetPrefab; // 생성할 프리팹
		private GameObject poolObject;

		public ObjectPool(PooledObject _targetPrefab, Transform parent, int _initsize = 5)
        {
            Init(_targetPrefab, parent, _initsize);
        }

		void Init(PooledObject _targetPrefab, Transform parent, int _initSize)
		{
			stack = new Stack<PooledObject>(_initSize);
			targetPrefab = _targetPrefab;
			poolObject = new GameObject($"{targetPrefab.name} Pool");
			poolObject.transform.parent = parent;

			for (int i = 0; i < _initSize; i++)
			{
				// 프리팹 생성
				CreatePooledObject();
			}
		}

		public PooledObject GetPool()
		{
			if (stack.Count == 0)
				CreatePooledObject();

			PooledObject pooledObject = stack.Pop();
			pooledObject.gameObject.SetActive(true);
			return pooledObject;
		}

		public void PushPool(PooledObject target)
		{
			target.transform.parent = poolObject.transform;
			target.gameObject.SetActive(false);
			stack.Push(target);
		}

		void CreatePooledObject()
		{
			PooledObject obj = MonoBehaviour.Instantiate(targetPrefab);
			obj.PooledInit(this);
			PushPool(obj);
		}
    }
}
