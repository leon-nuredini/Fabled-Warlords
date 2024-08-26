using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units;
using System;

public class Stronghold : LStructure
{
    public static event Action<Stronghold> OnAnyStrongholdClicked;
    private RecruitUnitAbility _recruitUnitAbility;
    private IncomeGenerationAbility _incomeGenerationAbility;
    private bool _isRuined;

    #region Properties

    public RecruitUnitAbility RecruitUnitAbility => _recruitUnitAbility;

    public IncomeGenerationAbility IncomeGenerationAbility => _incomeGenerationAbility;

    private RecruitAIAction _recruitAIAction;

    public bool IsRuined
    {
        get => _isRuined;
        set
        {
            _isRuined = value;
            if (_isRuined)
            {
                IncomeGenerationAbility.IncomeAmount = 1;
                Destroy(RecruitUnitAbility);
                _recruitUnitAbility = null;
                Destroy(_recruitAIAction);
                _recruitAIAction = null;
            }
        }
    }

    #endregion

    public override void Initialize()
    {
        base.Initialize();
        _recruitUnitAbility = GetComponent<RecruitUnitAbility>();
        _incomeGenerationAbility = GetComponent<IncomeGenerationAbility>();
        _recruitAIAction = GetComponentInChildren<RecruitAIAction>();
    }

    protected override void OnCapturedActionPerformed(LUnit aggressor)
    {
        if (!IsRuined) KillAllFriendlyUnits();
        base.OnCapturedActionPerformed(aggressor);
    }

    private void KillAllFriendlyUnits()
    {
        Player player = CellGrid.Instance.Players.First(player => player.PlayerNumber == PlayerNumber);
        List<Unit> unitList = CellGrid.Instance.GetPlayerUnits(player);
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i] is Stronghold stronghold && !stronghold.IsRuined)
                continue;

            if (unitList[i] is LStructure lStructure)
            {
                lStructure.AbandonStructure();
                continue;
            }

            unitList[i].KillInstantly();
        }
    }

    public override void OnMouseDown()
    {
        OnAnyStrongholdClicked?.Invoke(this);
        base.OnMouseDown();
    }
}