public class StormElemental : LUnit
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
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (ThunderStrikeSkill == null) return;
        float randomChance = UnityEngine.Random.Range(0f, 100f);
        if (randomChance < ThunderStrikeSkill.ProcChance) return;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(_thunderStrikeSkill.DurationInTurns);
    }
}
