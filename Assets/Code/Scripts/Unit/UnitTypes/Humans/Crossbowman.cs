using TbsFramework.Units;
using UnityEngine;

public class Crossbowman : LUnit, IRanged
{
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is IMounted) newDamage *= 1.5f;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
}
