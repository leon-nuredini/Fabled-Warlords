public class Village : LStructure
{
    private VillageHealingSkill     _villageHealingSkill;
    private IncomeGenerationAbility _incomeGenerationAbility;

    #region Properties

    public VillageHealingSkill VillageHealingSkill => _villageHealingSkill;

    public IncomeGenerationAbility IncomeGenerationAbility => _incomeGenerationAbility;

    #endregion

    public override void Initialize()
    {
        base.Initialize();
        _villageHealingSkill     = GetComponent<VillageHealingSkill>();
        _incomeGenerationAbility = GetComponent<IncomeGenerationAbility>();
    }
}