using NaughtyAttributes;
using UnityEngine;

public class StatusEffectType : ScriptableObject
{
    [SerializeField] private GameObject _effect;

    public GameObject Effect => _effect;
}
