using TbsFramework.Units;
using UnityEngine;

public class Treant : LUnit, IMonster
{
    private RootGraspSkill _rootGraspSkill;

    #region Properties

    public RootGraspSkill RootGraspSkill => _rootGraspSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _rootGraspSkill = GetComponent<RootGraspSkill>();
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
        if (RootGraspSkill == null) return;
        if (enemyUnit.HitPoints <= 0) return;
        float randomChance = Random.Range(0f, 100f);
        if (randomChance > RootGraspSkill.ProcChance) return;
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(RootGraspSkill.DurationInTurns + extraTurn);
        if (_rootGraspSkill is ISpawnableEffect spawnableEffect) spawnableEffect.SpawnEffect(enemyUnit.transform);
        InitializeSpikes(enemyUnit);
    }

    private void InitializeSpikes(LUnit enemyUnit)
    {
        if (RootGraspSkill.TreantSpike == null) return;
        RootGraspSkill.TreantSpike.EnemyUnit = enemyUnit;
    }
}