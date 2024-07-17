public class Druid : LUnit
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
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (PoisonHexSkill == null) return;
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Weaken>(PoisonHexSkill.DurationInTurns + extraTurn);
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Poison>(PoisonHexSkill.DurationInTurns + extraTurn);
    }
}
