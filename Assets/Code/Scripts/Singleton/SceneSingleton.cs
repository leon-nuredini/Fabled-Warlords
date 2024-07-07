using UnityEngine;

namespace Singleton
{
    public class SceneSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<T>();

                return _instance;
            }
        }

        private void Awake()
        {
            if (TryRemoveDuplicates()) return;
            SetUpInstance();
        }

        private static void SetUpInstance()
        {
            GameObject newGameObject = new GameObject();
            newGameObject.name = typeof(T).Name;
            _instance = newGameObject.AddComponent<T>();
        }

        private bool TryRemoveDuplicates()
        {
            if (_instance == null)
            {
                _instance = this as T;
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