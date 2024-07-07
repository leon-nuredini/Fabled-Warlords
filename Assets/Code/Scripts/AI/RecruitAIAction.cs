using System.Collections;
using System.Linq;
using TbsFramework.Example4;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Players.AI.Actions;
using TbsFramework.Units;
using UnityEngine;

public class RecruitAIAction : AIAction
{
    public override void InitializeAction(Player player, Unit unit, CellGrid cellGrid)
    {
        unit.GetComponent<RecruitUnitAbility>().OnAbilitySelected(cellGrid);
    }

    public override bool ShouldExecute(Player player, Unit unit, CellGrid cellGrid)
    {
        return unit.GetComponent<RecruitUnitAbility>() != null;
    }

    public override void CleanUp(Player player, Unit unit, CellGrid cellGrid)
    {
    }

    public override IEnumerator Execute(Player player, Unit unit, CellGrid cellGrid)
    {
        RecruitUnitAbility _unitRecruitAbility = unit.GetComponent<RecruitUnitAbility>();
        var availableUnits = _unitRecruitAbility.PrefabsList;
        foreach (var _unit in availableUnits)
        {
            /*var shouldSpawn = _unit.GetComponentsInChildren<SpawnCondition>()
                .Select(c => c.ShouldSpawn(cellGrid, _unit.GetComponent<Unit>(), player))
                .Aggregate((result, next) => result || next);*/
            var shouldSpawn = true;
            if (shouldSpawn)
            {
                _unitRecruitAbility.SelectedPrefab = _unit;
                yield return StartCoroutine(_unitRecruitAbility.AIExecute(cellGrid));
                break;
            }
        }

        yield return new WaitForSeconds(0.075f);
    }

    public override void Precalculate(Player player, Unit unit, CellGrid cellGrid)
    {
    }

    public override void ShowDebugInfo(Player player, Unit unit, CellGrid cellGrid)
    {
    }
}