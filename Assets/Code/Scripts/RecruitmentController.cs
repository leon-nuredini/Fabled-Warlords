using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Units;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TbsFramework.Example4;
using TbsFramework.Players;
using TbsFramework.Units.UnitStates;
using Random = System.Random;

public class RecruitmentController : MonoBehaviour
{
    public static event Action                  OnAnyNewUnitRecruited;
    public static event Action<LUnit, int, int> OnAnyPurchaseUnit;

    public static event Action<RecruitableUnits> OnAnyUpdateRecruitableUnits;

    [BoxGroup("Recruitable Units")] [SerializeField]
    private RecruitableUnits _recruitableUnits;

    [BoxGroup("Allow Recruiting")] [SerializeField]
    private bool _allowAIRecruiting = true;

    private LUnit _selectedUnit;

    private List<RecruitUnitAbility> _recruitUnitAbilityList = new List<RecruitUnitAbility>();
    private List<LUnit>              _aiUnitsList            = new List<LUnit>();
    private List<Cell>               _aiCellsList            = new List<Cell>();

    private void Awake() => OnAnyUpdateRecruitableUnits?.Invoke(_recruitableUnits);

    private void OnEnable()
    {
        RecruitUnitAbility.OnAnyRegisterUnitRecruitAbility += OnRegisterAIRecruitAbility;
        RecruitUnitAbility.OnAnyAIRecruitUnit              += OnAIRecruitUnits;
        UIUnitRecruitButton.OnAnySelectUnit                += OnSelectUnit;
        UITutorial.OnAnyAllowRecruitment                   += EnableAIRecruitment;
        LSquare.OnAnyRecruitUnit                           += OnRecruitUnitAtCell;
        CellGrid.Instance.TurnEnded                        += OnTurnEnd;
    }

    private void OnDisable()
    {
        RecruitUnitAbility.OnAnyRegisterUnitRecruitAbility -= OnRegisterAIRecruitAbility;
        RecruitUnitAbility.OnAnyAIRecruitUnit              -= OnAIRecruitUnits;
        UIUnitRecruitButton.OnAnySelectUnit                -= OnSelectUnit;
        UITutorial.OnAnyAllowRecruitment                   -= EnableAIRecruitment;
        LSquare.OnAnyRecruitUnit                           -= OnRecruitUnitAtCell;
        CellGrid.Instance.TurnEnded                        -= OnTurnEnd;
    }

    private void EnableAIRecruitment() => _allowAIRecruiting = true;

    private void OnAIRecruitUnits(List<LUnit> units, List<Cell> cells)
    {
        if (!_allowAIRecruiting) return;
        List<RecruitUnitAbility> recruitUnitAbilityList = _recruitUnitAbilityList
            .Where(ability => ability.UnitReference.PlayerNumber == CellGrid.Instance.CurrentPlayerNumber).ToList();

        for (int i = 0; i < units.Count; i++)
            _aiUnitsList.Add(units[i]);

        for (int i = 0; i < cells.Count; i++)
            _aiCellsList.Add(cells[i]);

        bool _canRecruitUnits = true;
        for (int i = 0; i < recruitUnitAbilityList.Count; i++)
        {
            if (!recruitUnitAbilityList[i].IsDoneSelectingUnitsToRecruit)
                _canRecruitUnits = false;
        }

        if (_canRecruitUnits)
        {
            _aiCellsList = ShuffleCellsList(_aiCellsList);
            for (int i = 0; i < recruitUnitAbilityList.Count; i++)
                recruitUnitAbilityList[i].IsDoneSelectingUnitsToRecruit = false;

            List<LUnit> aiRecruitUnitList = AISelectUnitsToRecruit(_aiUnitsList, _aiCellsList);
            for (int i = 0; i < aiRecruitUnitList.Count; i++)
            {
                if (_aiCellsList.Count == 0) break;
                LUnit lUnit = aiRecruitUnitList[i];
                OnSelectUnit(lUnit);
                int  cellIndex    = UnityEngine.Random.Range(0, _aiCellsList.Count);
                Cell selectedCell = _aiCellsList[cellIndex];
                OnRecruitUnitAtCell(selectedCell);
                _aiCellsList.Remove(selectedCell);
            }

            ClearAILists();
        }
    }

    private void OnTurnEnd(object sender, bool isNetworkInvoked) => ClearAILists();

    private void ClearAILists()
    {
        _aiUnitsList.Clear();
        _aiCellsList.Clear();
    }

    private List<LUnit> AISelectUnitsToRecruit(List<LUnit> lUnitList, List<Cell> cells)
    {
        int wealth = EconomyController.Instance.GetCurrentWealth(CellGrid.Instance.CurrentPlayerNumber);
        int random;

        List<LUnit> tempUnitList = new List<LUnit>();

        //todo: Refactor this spaghetti code
        for (int i = 0; i < lUnitList.Count; i++)
        {
            SpawnCondition[] _spawnConditionArray  = lUnitList[i].GetComponentsInChildren<SpawnCondition>();
            bool[]           areConditionsMetArray = new bool[_spawnConditionArray.Length];

            for (int j = 0; j < _spawnConditionArray.Length; j++)
            {
                areConditionsMetArray[j] = _spawnConditionArray[j]
                    .ShouldSpawn(CellGrid.Instance, lUnitList[i], CellGrid.Instance.CurrentPlayer);
            }

            int conditionsMetAmount = 0;
            for (int j = 0; j < areConditionsMetArray.Length; j++)
            {
                if (areConditionsMetArray[j]) conditionsMetAmount++;
            }

            if (conditionsMetAmount == areConditionsMetArray.Length)
                tempUnitList.Add(lUnitList[i]);
        }

        lUnitList = tempUnitList;
        //todo: Refactor this spaghetti code
        
        List<LUnit> selectedUnitsList = new List<LUnit>();
        while (lUnitList.Count > 0)
        {
            random = UnityEngine.Random.Range(0, lUnitList.Count);
            LUnit selectedUnit = lUnitList[random];
            selectedUnitsList.Add(selectedUnit);
            wealth -= selectedUnit.UnitStats.Cost;
            lUnitList.Remove(selectedUnit);
            lUnitList = lUnitList.Where(unit => wealth >= unit.UnitStats.Cost).ToList();
        }

        int cellsLimit = selectedUnitsList.Count - cells.Count;
        if (cellsLimit > 0)
        {
            List<LUnit> maxPossibleRecruitableUnitsList = new List<LUnit>();
            for (int i = 0; i < cellsLimit; i++)
                maxPossibleRecruitableUnitsList.Add(selectedUnitsList[i]);

            return maxPossibleRecruitableUnitsList;
        }

        return selectedUnitsList;
    }

    private List<Cell> ShuffleCellsList(List<Cell> cellsInRange)
    {
        Random random = new Random();
        cellsInRange = cellsInRange.OrderBy(x => random.Next()).ToList();
        return cellsInRange;
    }

    private void OnSelectUnit(LUnit selectedUnit) => _selectedUnit = selectedUnit;

    private void OnRecruitUnitAtCell(Cell cell, Player player = null)
    {
        var  unitGO      = Instantiate(_selectedUnit);
        Unit spawnedUnit = unitGO.GetComponent<Unit>();
        CellGrid.Instance.AddUnit(spawnedUnit.transform, cell, CellGrid.Instance.CurrentPlayer);
        spawnedUnit.OnTurnStart();
        if (player != null && player is HumanPlayer)
        {
            spawnedUnit.SetState(new UnitStateMarkedAsFinished(spawnedUnit));
            spawnedUnit.ActionPoints = 0;
            CellGrid.Instance.UpdateCurrentPlayerUnits();
        }
        OnAnyNewUnitRecruited?.Invoke();
        if (spawnedUnit is LUnit lUnit)
            OnAnyPurchaseUnit?.Invoke(lUnit, lUnit.PlayerNumber, -lUnit.UnitStats.Cost);
    }

    private void OnRegisterAIRecruitAbility(RecruitUnitAbility recruitUnitAbility) =>
        _recruitUnitAbilityList.Add(recruitUnitAbility);
}