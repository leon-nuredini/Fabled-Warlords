using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units;
using UnityEngine;

public class Stronghold : LStructure
{
    private RecruitUnitAbility      _recruitUnitAbility;
    private IncomeGenerationAbility _incomeGenerationAbility;

    #region Properties
    public RecruitUnitAbility RecruitUnitAbility => _recruitUnitAbility;

    public IncomeGenerationAbility IncomeGenerationAbility => _incomeGenerationAbility;

    #endregion

    public override void Initialize()
    {
        base.Initialize();
        _recruitUnitAbility      = GetComponent<RecruitUnitAbility>();
        _incomeGenerationAbility = GetComponent<IncomeGenerationAbility>();
    }

    protected override void OnCapturedActionPerformed(LUnit aggressor)
    {
        KillAllFriendlyUnits();
        base.OnCapturedActionPerformed(aggressor);
    }

    private void KillAllFriendlyUnits()
    {
        Player     player   = CellGrid.Instance.Players.First(player => player.PlayerNumber == PlayerNumber);
        List<Unit> unitList = CellGrid.Instance.GetPlayerUnits(player);
        for (int i = 0; i < unitList.Count; i++)
        {
            if (unitList[i] is Stronghold)
                continue;
            
            if (unitList[i] is LStructure lStructure)
            {
                lStructure.AbandonStructure();
                continue;
            }

            unitList[i].KillInstantly();
        }
    }
}