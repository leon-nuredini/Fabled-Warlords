using Singleton;
using TMPro;
using UnityEngine;

public abstract class BaseTextSpawner : SceneSingleton<BaseTextSpawner>
{
    [SerializeField] protected GameObject _textGameObject;

    public abstract void SpawnTextGameObject(Vector3 spawnPosition, string text = "");
}