using TbsFramework.Units;
using UnityEngine;

public class StormElemental : LUnit, IMage
{
    private ThunderStrikeSkill _thunderStrikeSkill;

    #region Properties

    public ThunderStrikeSkill ThunderStrikeSkill => _thunderStrikeSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _thunderStrikeSkill = GetComponent<ThunderStrikeSkill>();
    }

    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= lUnit.UnitClassCounter.VSMageFactor;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }

    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (isEnemyTurn) return;
        if (ThunderStrikeSkill == null) return;
        if (enemyUnit.HitPoints <= 0) return;
        float randomChance = Random.Range(0f, 100f);
        if (randomChance > ThunderStrikeSkill.ProcChance) return;
        if (ThunderStrikeSkill is ISpawnableEffect spawnableEffect) spawnableEffect.SpawnEffect(enemyUnit.transform);
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(ThunderStrikeSkill.DurationInTurns);
    }
}