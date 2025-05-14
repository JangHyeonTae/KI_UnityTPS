using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DesignPattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour//singleton���� � ������ �޾ƿ��� �𸣹Ƿ� generic
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
                _instance = this as T; // GetComponent<T>(); GetComponent -���ſ�
                //this as T�� ���� : this�� ����Ұ�� �̱��� class�� ��ӹް�, T�� ����ϸ� �̱��� ~~�� ���
                DontDestroyOnLoad(_instance);
            }
        }
    }
}

