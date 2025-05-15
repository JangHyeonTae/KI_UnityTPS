using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    public abstract class PooledObject : MonoBehaviour
    {
        //ObjectPool -> PushPool�Լ� �ڵ����� �Ǵ����� �������
        
        //ObjectPool���� PooledObject�� ���
        //���� �������� �������ֱ����� ��
        public ObjectPool ObjPool { get; private set;  }

        public void PooledInit(ObjectPool objPool)
        {
            ObjPool = objPool;
        }

        public void ReturnPool()
        {
            ObjPool.PushPool(this);
        }
    }
}

