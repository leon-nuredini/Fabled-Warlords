using Lean.Pool;
using Singleton;
using UnityEngine;

public class EvadedTextSpawner : SceneSingleton<EvadedTextSpawner>
{
    [SerializeField] protected GameObject _textGameObject;
    [SerializeField] private TextOutlineColors _textOutlineColors;
    
    private readonly string _outlineColor = "_OutlineColor";
    
    public void SpawnTextGameObject(Vector3 spawnPosition)
    {
        if (_textGameObject == null) return;
        GameObject gObj = LeanPool.Spawn(_textGameObject, spawnPosition, Quaternion.identity);
        
        Material material = gObj.GetComponent<MeshRenderer>().material;
        material.SetColor(_outlineColor, _textOutlineColors.MissColor);
    }
}