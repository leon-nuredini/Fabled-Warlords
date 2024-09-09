using TbsFramework.Units;
using UnityEngine;

public class Leafshooter : LUnit, IRanged
{
    private RapidShotSkill _rapidShotSkill;
    private PoisonSkill _poisonSkill;
    
    #region Properties

    public RapidShotSkill RapidShotSkill => _rapidShotSkill;
    public PoisonSkill PoisonSkill => _poisonSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _rapidShotSkill = GetComponent<RapidShotSkill>();
        _poisonSkill = GetComponent<PoisonSkill>();
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
            return enemyUnit.UnitClassCounter.VSRangedFactor;
        return 1f;
    }
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (enemyUnit is LStructure) return;
        if (PoisonSkill == null) return;
        if (enemyUnit.HitPoints <= 0) return;
        float randomChance = Random.Range(0f, 100f);
        if (randomChance > PoisonSkill.ProcChance) return;
        if (PoisonSkill is ISpawnableEffect spawnableEffect) spawnableEffect.SpawnEffect(enemyUnit.transform);
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Poison>(_poisonSkill.DurationInTurns + extraTurn);
    }
}
