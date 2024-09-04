using Lean.Pool;
using Singleton;
using UnityEngine;

public class DamageTextSpawner : SceneSingleton<DamageTextSpawner>
{
    [SerializeField] protected GameObject _textGameObject;
    [SerializeField] private TextOutlineColors _textOutlineColors;
    
    private readonly string _outlineColor = "_OutlineColor";

    public void SpawnTextGameObject(Vector3 spawnPosition, string damage = "")
    {
        if (_textGameObject == null) return;
        GameObject gObj = LeanPool.Spawn(_textGameObject, spawnPosition, Quaternion.identity);
        if (gObj.TryGetComponent(out DamageText damageText))
            damageText.UpdateTextValue(damage);
        
        Material material = gObj.GetComponent<MeshRenderer>().material;
        material.SetColor(_outlineColor, _textOutlineColors.DamageColor);
    }
}