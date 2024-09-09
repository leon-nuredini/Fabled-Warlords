using TbsFramework.Units;
using UnityEngine;

public class Paladin : LUnit, ISwordInfantry
{
    private VictorsSmiteSkill _victorsSmiteSkill;
    
    #region Properties

    public VictorsSmiteSkill VictorsSmiteSkill => _victorsSmiteSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _victorsSmiteSkill = GetComponent<VictorsSmiteSkill>();
    }
    
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= GetClassCounterDamageFactor(lUnit);

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
    
    public override float GetClassCounterDamageFactor(LUnit enemyUnit)
    {
        if (enemyUnit.UnitClassCounter != null)
            return enemyUnit.UnitClassCounter.VSSwordInfantryCounter;
        return 1f;
    }
}
