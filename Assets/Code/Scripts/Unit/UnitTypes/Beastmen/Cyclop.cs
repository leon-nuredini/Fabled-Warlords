using TbsFramework.Units;
using UnityEngine;

public class Cyclop : LUnit, IMonster
{
    private OverpowerSkill _overpowerSkill;
    
    #region Properties

    public OverpowerSkill OverpowerSkill => _overpowerSkill;

    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _overpowerSkill = GetComponent<OverpowerSkill>();
    }
    
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= lUnit.UnitClassCounter.VSMonsterFactor;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (OverpowerSkill == null) return;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(OverpowerSkill.DurationInTurns);
    }
}
