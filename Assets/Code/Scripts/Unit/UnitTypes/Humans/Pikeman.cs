using TbsFramework.Units;
using UnityEngine;

public class Pikeman : LUnit, ISpearInfantry
{
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= lUnit.UnitClassCounter.VSSpearInfantryCounter;
        
        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
}
