using TbsFramework.Units;
using UnityEngine;

public class Monk : LUnit, IMage
{
    private HexSkill _hexSkill;
    
    #region Properties

    public HexSkill HexSkill => _hexSkill;
    
    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _hexSkill = GetComponent<HexSkill>();
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
            return enemyUnit.UnitClassCounter.VSMageFactor;
        return 1f;
    }

    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (enemyUnit is LStructure) return;
        if (HexSkill == null) return;
        if (HexSkill is ISpawnableEffect spawnableEffect) spawnableEffect.SpawnEffect(enemyUnit.transform);
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Weaken>(_hexSkill.DurationInTurns + extraTurn);
    }
}
