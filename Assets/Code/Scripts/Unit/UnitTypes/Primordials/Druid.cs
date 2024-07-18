using TbsFramework.Units;
using UnityEngine;

public class Druid : LUnit, IMage
{
    private PoisonHexSkill _poisonHexSkill;
    
    #region Properties

    public PoisonHexSkill PoisonHexSkill => _poisonHexSkill;
    
    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _poisonHexSkill = GetComponent<PoisonHexSkill>();
    }
    
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is IMonster) newDamage *= 1.25f;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (PoisonHexSkill == null) return;
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Weaken>(PoisonHexSkill.DurationInTurns + extraTurn);
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Poison>(PoisonHexSkill.DurationInTurns + extraTurn);
    }
}
