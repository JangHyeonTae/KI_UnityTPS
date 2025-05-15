using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    //�����ϴ� ����� ���� �ٽ�
    //�׽�Ʈ�� monobehaviour ��ӹ��� Ŭ����
    public class ObjectPool
    {
        private Stack<PooledObject> _stack;
        private PooledObject _targetPrefab;
        private GameObject _poolObject;

        public ObjectPool(Transform parent,PooledObject targetPrefab, int initSize = 5) => Init( parent,targetPrefab, initSize);

        //1
        private void Init(Transform parent,PooledObject targetPrefab, int initSize)
        {
            _stack = new Stack<PooledObject>(initSize);
            _targetPrefab = targetPrefab;
            //�ν����Ϳ��� ������Ʈ�� �����ǰ� �θ� �Ǵ� ������Ʈ ����
            _poolObject = new GameObject($"{targetPrefab.name} Pool");
            _poolObject.transform.parent = parent;

            //5
            for (int i = 0; i < initSize; i++)
            {
                CreatePooledObject();
            }
        }

        //4
        public PooledObject PopPool()
        {
            //������� �� ���������ϸ� ���� ����
            if (_stack.Count == 0) CreatePooledObject();

            PooledObject pooledObject = _stack.Pop();
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        //3
        public void PushPool(PooledObject target)
        {
            //�ٽ�
            target.transform.parent = _poolObject.transform;
            target.gameObject.SetActive(false);
            _stack.Push(target);
        }

        //2
        private void CreatePooledObject()
        {
            PooledObject obj = MonoBehaviour.Instantiate(_targetPrefab);
            obj.PooledInit(this);
            PushPool(obj);
        }
    }
}

