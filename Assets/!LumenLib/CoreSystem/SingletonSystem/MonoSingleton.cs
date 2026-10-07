using UnityEngine;

namespace _LumenLib.CoreSystem.SingletonSystem
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindFirstObjectByType<T>();
                }

                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = (T)this;
            OnAwake();
        }

        protected virtual void OnDestroy()
        {
            if(_instance == this)
                _instance = null;

            Destroy();
        }

        protected virtual void OnAwake()
        {
            
        }

        protected virtual void Destroy()
        {
            
        }
    }
}
