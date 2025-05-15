using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    public abstract class PooledObject : MonoBehaviour
    {
        //ObjectPool -> PushPool함수 자동으로 되는형식 만들거임
        
        //ObjectPool에서 PooledObject가 어디에
        //속한 형식인지 지정해주기위한 것
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

