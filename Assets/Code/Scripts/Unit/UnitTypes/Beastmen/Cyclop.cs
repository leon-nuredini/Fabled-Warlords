using TbsFramework.Units;
using UnityEngine;

public class Cyclop : LUnit, IMonster
{
    private StunSkill _stunSkill;
    
    #region Properties

    public StunSkill StunSkill => _stunSkill;

    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _stunSkill = GetComponent<StunSkill>();
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
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (enemyUnit is LStructure) return;
        if (isEnemyTurn) return;
        if (StunSkill == null) return;
        if (enemyUnit.HitPoints <= 0) return;
        float randomChance = Random.Range(0f, 100f);
        if (randomChance > StunSkill.ProcChance) return;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(StunSkill.DurationInTurns);
    }
}
