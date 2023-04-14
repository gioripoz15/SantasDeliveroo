using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gioripoz {
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        //Classe generica per i singleton
        private static T instance;

        public static T Instance
        {
            get
            {
                //Debug.Assert(instance != null);
                if (instance != null)
                {
                    return instance;
                }
                else
                {
                    return null;
                }
            }
        }

        public void Awake()
        {
            instance = this as T;
        }
    }
}
