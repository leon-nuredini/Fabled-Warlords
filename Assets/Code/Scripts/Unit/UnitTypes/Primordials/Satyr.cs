public class Satyr : Pikeman
{
    private PoisonSkill _poisonSkill;
    
    #region Properties
    
    public PoisonSkill PoisonSkill => _poisonSkill;

    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _poisonSkill = GetComponent<PoisonSkill>();
    }
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (enemyUnit is LStructure) return;
        if (PoisonSkill == null) return;
        if (enemyUnit.HitPoints <= 0) return;
        float randomChance = UnityEngine.Random.Range(0f, 100f);
        if (randomChance > PoisonSkill.ProcChance) return;
        if (PoisonSkill is ISpawnableEffect spawnableEffect) spawnableEffect.SpawnEffect(enemyUnit.transform);
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Poison>(PoisonSkill.DurationInTurns + extraTurn);
    }
}
