using TbsFramework.Units;
using UnityEngine;

public class Troll : LUnit, IMonster
{
    private RegenerationSkill _regenerationSkill;

    #region Properties

    public RegenerationSkill RegenerationSkill => _regenerationSkill;

    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _regenerationSkill = GetComponent<RegenerationSkill>();
    }
    
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is ISpearInfantry) newDamage *= 1.5f;
        if (other is IRanged) newDamage *= 1.25f;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
}