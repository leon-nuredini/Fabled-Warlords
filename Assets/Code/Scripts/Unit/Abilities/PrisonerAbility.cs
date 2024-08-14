using System.Collections.Generic;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class PrisonerAbility : Ability
{
    [SerializeField] private List<LUnit> _overlordUnitList;

    [BoxGroup("Unit Stats")] [SerializeField]
    private UnitStats _releasedUnitStats;

    [BoxGroup("Graphics")] [SerializeField]
    private SpriteRenderer _mask;

    [BoxGroup("Prisoner Color Tolerance Amount")] [SerializeField]
    private float _prisonerColorTintAmount = 0f;

    private readonly string _colorChangeTolerance = "_ColorChangeTolerance";

    private Material _material;

    private bool _isPrisoner = true;

    public bool IsPrisoner => _isPrisoner;

    private void Awake()
    {
        _material = _mask.material;
        _material.SetFloat(_colorChangeTolerance, _prisonerColorTintAmount);
    }

    private void OnEnable()
    {
        for (int i = 0; i < _overlordUnitList.Count; i++)
            _overlordUnitList[i].OnDie += ReleasePrisoner;
    }

    private void ReleasePrisoner(UnitDirection direction)
    {
        List<LUnit> tempUnits = _overlordUnitList;
        for (int i = 0; i < tempUnits.Count; i++)
        {
            if (tempUnits[i] == null || tempUnits[i].HitPoints <= 0)
                _overlordUnitList.Remove(tempUnits[i]);
        }

        _overlordUnitList = tempUnits;

        if (_overlordUnitList.Count > 0) return;

        _material.SetFloat(_colorChangeTolerance, 1f);
        _isPrisoner = false;
        if (UnitReference is LUnit lUnit)
        {
            lUnit.UnitStats = _releasedUnitStats;
            lUnit.UpdateUnitStats();
        }

        UnitReference.PlayerNumber = 0;
    }

    public override void OnTurnStart(CellGrid cellGrid)
    {
        base.OnTurnStart(cellGrid);
        if (cellGrid.CurrentPlayerNumber != 0) return;
        UnitReference.PlayerNumber = 0;
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        base.OnTurnEnd(cellGrid);
        if (_isPrisoner && cellGrid.CurrentPlayerNumber == 0)
        {
            UnitReference.PlayerNumber = 1;
            return;
        }

        UnitReference.PlayerNumber = 0;
    }
}