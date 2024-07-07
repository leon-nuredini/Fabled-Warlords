using UnityEngine;

namespace Singleton
{
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();
                
                if (_instance == null)
                    SetUpInstance();

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (TryRemoveDuplicates()) return;
        }

        private static void SetUpInstance()
        {
            GameObject newGameObject = new GameObject();
            newGameObject.name = typeof(T).Name;
            _instance = newGameObject.AddComponent<T>();
            DontDestroyOnLoad(newGameObject);
        }

        private bool TryRemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return true;
            }

            return false;
        }
    }
}