using System;

public class Barrack : LStructure
{
    public static event Action<Barrack> OnAnyBarrackClicked;

    private RecruitUnitAbility _recruitUnitAbility;

    #region Properties

    public RecruitUnitAbility RecruitUnitAbility => _recruitUnitAbility;

    #endregion

    public override void Initialize()
    {
        base.Initialize();
        _recruitUnitAbility = GetComponent<RecruitUnitAbility>();
    }

    public override void OnMouseDown()
    {
        OnAnyBarrackClicked?.Invoke(this);
        base.OnMouseDown();
    }
}