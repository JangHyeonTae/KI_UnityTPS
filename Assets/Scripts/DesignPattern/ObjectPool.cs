using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    //참조하는 오디오 설명 다시
    //테스트로 monobehaviour 상속받은 클래스
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
            //인스펙터에서 오브젝트가 생성되고 부모가 되는 오브젝트 생성
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
            //비어있을 때 가져갈려하면 새로 생성
            if (_stack.Count == 0) CreatePooledObject();

            PooledObject pooledObject = _stack.Pop();
            pooledObject.gameObject.SetActive(true);
            return pooledObject;
        }

        //3
        public void PushPool(PooledObject target)
        {
            //다시
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

