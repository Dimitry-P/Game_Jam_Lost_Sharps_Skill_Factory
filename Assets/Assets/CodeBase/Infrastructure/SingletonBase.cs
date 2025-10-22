using UnityEngine;
using System.Collections;
using System.Collections.Generic;


    [DisallowMultipleComponent]
    public abstract class SingletonBase<T> : MonoBehaviour where T:MonoBehaviour
    {
        [Header("Singleton")]
        [SerializeField] private bool m_DoNotDestroyOnLoad;

        public static T Instance {  get; private set; } 

        protected virtual void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return; 
            }
            Instance = this as T;

            if (m_DoNotDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }


