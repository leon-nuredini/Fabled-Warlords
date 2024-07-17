public class Pixie : LUnit
{
    private SleepSkill _sleepSkill;
    
    #region Properties

    public SleepSkill SleepSkill => _sleepSkill;

    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _sleepSkill = GetComponent<SleepSkill>();
    }
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (SleepSkill == null) return;
        float randomChance = UnityEngine.Random.Range(0f, 100f);
        if (randomChance < SleepSkill.ProcChance) return;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(SleepSkill.DurationInTurns);
    }
}
