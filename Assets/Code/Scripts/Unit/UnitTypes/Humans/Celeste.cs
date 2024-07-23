using TbsFramework.Units;
using UnityEngine;

public class Celeste : LUnit, ISwordInfantry
{
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= lUnit.UnitClassCounter.VSSwordInfantryCounter;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
}
