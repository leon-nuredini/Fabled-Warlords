using UnityEngine;

public class GameObjectAwakeDisabler : MonoBehaviour
{
    private void Awake() => gameObject.SetActive(false);
}