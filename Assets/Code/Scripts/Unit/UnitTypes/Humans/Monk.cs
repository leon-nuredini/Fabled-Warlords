public class Monk : LUnit
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

    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (HexSkill == null) return;
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Weaken>(_hexSkill.DurationInTurns + extraTurn);
    }
}
