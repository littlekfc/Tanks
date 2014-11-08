using UnityEngine;
using System.Collections;

namespace Tanks
{
    public class Singleton<T> : TBehaviour where T : class
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = this as T;
            else
                Destroy(gameObject);
        }
    }
}