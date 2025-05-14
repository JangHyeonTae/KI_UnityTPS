using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour//singleton으로 어떤 파일을 받아올지 모르므로 generic
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    DontDestroyOnLoad(_instance);
                }
                return _instance;
            }
        }

        protected void SingletonInit()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this as T; // GetComponent<T>(); GetComponent -무거움
                //this as T의 이유 : this만 사용할경우 싱글턴 class만 상속받고, T를 사용하면 싱글턴 ~~를 상속
                DontDestroyOnLoad(_instance);
            }
        }
    }
}

