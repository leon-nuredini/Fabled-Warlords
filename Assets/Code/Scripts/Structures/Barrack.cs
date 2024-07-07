using System;

public class Barrack : LStructure
{
    private RecruitUnitAbility _recruitUnitAbility;

    #region Properties

    public RecruitUnitAbility RecruitUnitAbility => _recruitUnitAbility;

    #endregion

    public override void Initialize()
    {
        base.Initialize();
        _recruitUnitAbility = GetComponent<RecruitUnitAbility>();
    }
}