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
    
    protected override void AttackActionPerformed(float actionCost)
    {
        if (_rapidShotSkill != null) _rapidShotSkill.AddAdditionalActionPoint();
        base.AttackActionPerformed(actionCost);
    }
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (PoisonSkill == null) return;
        float randomChance = UnityEngine.Random.Range(0f, 100f);
        if (randomChance < PoisonSkill.ProcChance) return;
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Poison>(_poisonSkill.DurationInTurns + extraTurn);
    }
}
