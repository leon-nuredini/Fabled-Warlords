using TbsFramework.Units;
using UnityEngine;

public class StormElemental : LUnit, IMage
{
    private ThunderStrikeSkill _thunderStrikeSkill;
    private SpiritWardSkill _spiritWardSkill;

    #region Properties

    public ThunderStrikeSkill ThunderStrikeSkill => _thunderStrikeSkill;
    public SpiritWardSkill SpiritWardSkill => _spiritWardSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _thunderStrikeSkill = GetComponent<ThunderStrikeSkill>();
        _spiritWardSkill = GetComponent<SpiritWardSkill>();

        if (_spiritWardSkill != null)
            StatusEffectsController.CanApplyStatusEffects = false;
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
        if (enemyUnit is LStructure) return;
        if (isEnemyTurn) return;
        if (ThunderStrikeSkill == null) return;
        if (enemyUnit.HitPoints <= 0) return;
        float randomChance = Random.Range(0f, 100f);
        if (randomChance > ThunderStrikeSkill.ProcChance) return;
        if (ThunderStrikeSkill is ISpawnableEffect spawnableEffect) spawnableEffect.SpawnEffect(enemyUnit.transform);
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(ThunderStrikeSkill.DurationInTurns);
    }
}