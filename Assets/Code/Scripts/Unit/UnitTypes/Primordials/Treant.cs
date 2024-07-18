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
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (RootGraspSkill == null) return;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(RootGraspSkill.DurationInTurns);
    }
}
