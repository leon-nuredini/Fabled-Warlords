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
        if (other is IMonster) newDamage *= 1.5f;
        if (other is IMounted) newDamage *= 1.25f;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
}
