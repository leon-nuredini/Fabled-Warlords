using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffect", menuName = "StatusEffects/StatusEffectType/Weaken", order = 0)]
public class Weaken : StatusEffectType
{
    public float weakenFactor = 1.5f;
}
