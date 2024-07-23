using TbsFramework.Units;
using UnityEngine;

public class Crossbowman : LUnit, IRanged
{
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= lUnit.UnitClassCounter.VSRangedFactor;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
}
