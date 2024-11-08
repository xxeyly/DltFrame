using System;
using UnityEngine;

namespace DltFramework
{
    public abstract class SingletonBaseWindow<T> : BaseWindow where T : MonoBehaviour
    {
        private static T _instance;
        
        [Obsolete("Obsolete")]
        
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                }

                return _instance;
            }
        }
    }
}