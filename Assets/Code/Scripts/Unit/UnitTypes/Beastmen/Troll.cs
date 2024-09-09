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
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= GetClassCounterDamageFactor(lUnit);

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
    
    public override float GetClassCounterDamageFactor(LUnit enemyUnit)
    {
        if (enemyUnit.UnitClassCounter != null)
            return enemyUnit.UnitClassCounter.VSMonsterFactor;
        return 1f;
    }
}