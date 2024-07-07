using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class RecruitUnitAbility : Ability, ISkill
{
    public static event Action<RecruitUnitAbility> OnAnyRegisterUnitRecruitAbility;
    public static event Action<List<LUnit>, List<Cell>> OnAnyAIRecruitUnit;

    [BoxGroup] [SerializeField] private string _skillName;
    [BoxGroup] [SerializeField] private string _skillDescription;

    [SerializeField] private List<GameObject> _prefabsListRed;
    [SerializeField] private List<GameObject> _prefabsListBlue;
    [SerializeField] private List<GameObject> _prefabsListGreen;

    private List<LUnit> _redUnitList = new List<LUnit>();
    private List<LUnit> _blueUnitList = new List<LUnit>();
    private List<LUnit> _greenUnitList = new List<LUnit>();

    [SerializeField] private GameObject _selectedPrefab;

    [SerializeField] private int _recruitRange = 1;

    private List<Cell> _cellsInRange = new List<Cell>();

    [SerializeField] private bool _isDoneSelectingUnitsToRecruit;

    #region Properties

    public List<GameObject> PrefabsList
    {
        get
        {
            LStructure barrack = UnitReference as LStructure;
            List<GameObject> unitsList = new List<GameObject>();
            if (barrack != null)
                switch (barrack.Faction)
                {
                    case UnitFaction.Red:
                        unitsList = _prefabsListRed;
                        break;
                    case UnitFaction.Green:
                        unitsList = _prefabsListGreen;
                        break;
                    case UnitFaction.Blue:
                        unitsList = _prefabsListBlue;
                        break;
                }

            return unitsList;
        }
    }

    public GameObject SelectedPrefab
    {
        get => _selectedPrefab;
        set => _selectedPrefab = value;
    }

    public bool IsDoneSelectingUnitsToRecruit
    {
        get => _isDoneSelectingUnitsToRecruit;
        set => _isDoneSelectingUnitsToRecruit = value;
    }

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    #endregion

    private void Awake()
    {
        UpdateUnitList(_prefabsListRed, ref _redUnitList);
        UpdateUnitList(_prefabsListBlue, ref _blueUnitList);
        UpdateUnitList(_prefabsListGreen, ref _greenUnitList);
    }

    private void Start() => OnAnyRegisterUnitRecruitAbility?.Invoke(this);

    private void UpdateUnitList(List<GameObject> gameObjects, ref List<LUnit> unitList)
    {
        for (int i = 0; i < gameObjects.Count; i++)
            if (gameObjects[i].TryGetComponent(out LUnit lUnit))
                unitList.Add(lUnit);
    }

    private void OnEnable()
    {
        RecruitmentController.OnAnyUpdateRecruitableUnits += UpdateRecruitableUnits;
        UIRecruitment.OnAnyClickRecruitButton += Act;
        RecruitmentController.OnAnyNewUnitRecruited += UnmarkCells;
        LUnit.OnAnyUnitClicked += UnmarkCells;
        LSquare.OnAnyClickCell += UnmarkCells;
        UITop.OnAnyRecruitButtonClicked += UnmarkCells;
        UITop.OnAnyMenuButtonClicked += UnmarkCells;
    }

    private void OnDisable()
    {
        RecruitmentController.OnAnyUpdateRecruitableUnits -= UpdateRecruitableUnits;
        UIRecruitment.OnAnyClickRecruitButton -= Act;
        RecruitmentController.OnAnyNewUnitRecruited -= UnmarkCells;
        LUnit.OnAnyUnitClicked -= UnmarkCells;
        LSquare.OnAnyClickCell -= UnmarkCells;
        UITop.OnAnyRecruitButtonClicked -= UnmarkCells;
        UITop.OnAnyMenuButtonClicked -= UnmarkCells;
    }

    private void UpdateRecruitableUnits(RecruitableUnits recruitableUnits)
    {
        FilterUnitList(ref _redUnitList, recruitableUnits);
        FilterUnitList(ref _blueUnitList, recruitableUnits);
        FilterUnitList(ref _greenUnitList, recruitableUnits);
    }

    private void FilterUnitList(ref List<LUnit> unitList, RecruitableUnits recruitableUnits)
    {
        List<LUnit> filteredUnitList = new List<LUnit>();

        for (int i = 0; i < unitList.Count; i++)
        {
            switch (unitList[i])
            {
                case Archer archer:
                    if (recruitableUnits.CanRecruitArcher)
                        filteredUnitList.Add(unitList[i]);
                    break;
                case Assassin assassin:
                    if (recruitableUnits.CanRecruitAssassin)
                        filteredUnitList.Add(unitList[i]);
                    break;
                case AxeKnight axeKnight:
                    if (recruitableUnits.CanRecruitAxeKnight)
                        filteredUnitList.Add(unitList[i]);
                    break;
                case Berserker berserker:
                    if (recruitableUnits.CanRecruitBerserker)
                        filteredUnitList.Add(unitList[i]);
                    break;
                case LanceKnight lanceKnight:
                    if (recruitableUnits.CanRecruitLanceKnight)
                        filteredUnitList.Add(unitList[i]);
                    break;
                case Spearman spearman:
                    if (recruitableUnits.CanRecruitSpearman)
                        filteredUnitList.Add(unitList[i]);
                    break;
                case Swordsman swordsman:
                    if (recruitableUnits.CanRecruitSwordsman)
                        filteredUnitList.Add(unitList[i]);
                    break;
                case Wizard wizard:
                    if (recruitableUnits.CanRecruitWizard)
                        filteredUnitList.Add(unitList[i]);
                    break;
            }
        }

        unitList = filteredUnitList;
    }

    public void Act()
    {
        if (CellGrid.Instance == null) return;
        if (UnitReference.PlayerNumber != CellGrid.Instance.CurrentPlayerNumber) return;
        StartCoroutine(Act(CellGrid.Instance, false));
    }

    public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
    {
        IsDoneSelectingUnitsToRecruit = false;
        if (cellGrid.CurrentPlayer is AIPlayer)
        {
            List<Cell> cells = AIGetSpawnCells(cellGrid);
            if (cells == null) yield break;

            UnitFaction unitFaction;
            if (UnitReference is LUnit lUnit)
            {
                unitFaction = lUnit.Faction;
                List<LUnit> recruitableUnits = new List<LUnit>();
                int wealth = EconomyController.Instance.GetCurrentWealth(cellGrid.CurrentPlayerNumber);
                switch (unitFaction)
                {
                    case UnitFaction.Red:
                        recruitableUnits = _redUnitList
                            .Where(unit => unit.Faction == unitFaction && wealth >= unit.UnitStats.Cost).ToList();
                        break;
                    case UnitFaction.Green:
                        recruitableUnits = _greenUnitList
                            .Where(unit => unit.Faction == unitFaction && wealth >= unit.UnitStats.Cost).ToList();
                        break;
                    case UnitFaction.Blue:
                        recruitableUnits = _blueUnitList
                            .Where(unit => unit.Faction == unitFaction && wealth >= unit.UnitStats.Cost).ToList();
                        break;
                }

                if (recruitableUnits.Count == 0) yield break;
                IsDoneSelectingUnitsToRecruit = true;
                OnAnyAIRecruitUnit?.Invoke(recruitableUnits, cells);
            }
        }
        else
        {
            HighlightAvailableCells(cellGrid);
        }

        yield return null;
    }

    private List<Cell> AIGetSpawnCells(CellGrid cellGrid)
    {
        _cellsInRange =
            cellGrid.Cells.Where(cell => cell.GetDistance(UnitReference.Cell) <= _recruitRange && !cell.IsTaken)
                .ToList();

        if (_cellsInRange.Count == 0) return null;
        return _cellsInRange;
    }

    private void HighlightAvailableCells(CellGrid cellGrid)
    {
        _cellsInRange =
            cellGrid.Cells.Where(cell => cell.GetDistance(UnitReference.Cell) <= _recruitRange && !cell.IsTaken)
                .ToList();

        for (int i = 0; i < _cellsInRange.Count(); i++)
            if (_cellsInRange[i] is LSquare lSquare)
                lSquare.MarkAsRecruitZone();
    }

    private void UnmarkCells(LSquare lSquare) => UnmarkCells();

    private void UnmarkCells()
    {
        if (UnitReference.PlayerNumber != CellGrid.Instance.CurrentPlayerNumber) return;
        for (int i = 0; i < _cellsInRange.Count(); i++)
            if (_cellsInRange[i] is LSquare lSquare)
                lSquare.UnMark();
    }
}