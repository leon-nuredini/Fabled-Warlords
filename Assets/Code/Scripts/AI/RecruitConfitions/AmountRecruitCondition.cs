using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Example4;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units;
using UnityEngine;

public class AmountRecruitCondition : SpawnCondition
{
    public int TargetAmount;

    public override bool ShouldSpawn(CellGrid cellGrid, Unit unit, Player player)
    {
        /*var amount = cellGrid.GetPlayerUnits(player)
            .Where(u => u.GetComponent<LUnit>().UnitDetails.UnitName
                       .Equals(unit.GetComponent<LUnit>().UnitDetails.UnitName))
            .Count();*/

        var amount = cellGrid.GetPlayerUnits(player)
            .Where(u => u.GetType().Equals(unit.GetType()))
            .Count();

        /*if (amount < TargetAmount)
            Debug.LogError($"Amount of {unit.gameObject.name} : {amount}");*/
        return amount < TargetAmount;
    }
}