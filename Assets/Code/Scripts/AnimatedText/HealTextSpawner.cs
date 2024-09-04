using Lean.Pool;
using Singleton;
using UnityEngine;

public class HealTextSpawner : SceneSingleton<HealTextSpawner>
{
    [SerializeField] protected GameObject _textGameObject;
    [SerializeField] private TextOutlineColors _textOutlineColors;
    
    private readonly string _outlineColor = "_OutlineColor";

    public void SpawnTextGameObject(Vector3 spawnPosition, string healAmount = "")
    {
        if (_textGameObject == null) return;
        GameObject gObj = LeanPool.Spawn(_textGameObject, spawnPosition, Quaternion.identity);
        if (gObj.TryGetComponent(out HealText healText))
            healText.UpdateTextValue($"+{healAmount}");

        Material material = gObj.GetComponent<MeshRenderer>().material;
        material.SetColor(_outlineColor, _textOutlineColors.HealColor);
    }
}
