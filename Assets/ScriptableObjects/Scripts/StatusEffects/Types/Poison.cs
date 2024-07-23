using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffects/StatusEffectType/Poison", order = 1)]
public class Poison : StatusEffectType
{
    public float damageFactor = 1.15f;
}
