using TbsFramework.Units;
using UnityEngine;

public class Celeste : LUnit, ISwordInfantry
{
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is IMonster) newDamage *= 1.5f;
        if (other is IMounted) newDamage *= 1.25f;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
}
