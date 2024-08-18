using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class ClaimAbility : Ability
{
    public event Action OnClaimStructure;
    
    [SerializeField] private int _playerUnitRange = 12;
    [SerializeField] private List<ClaimAbility> _structuresList = new List<ClaimAbility>();

    [BoxGroup("Flag Color")] [SerializeField]
    private SpriteRenderer _flagImage;

    [BoxGroup("Flag Color")] [SerializeField]
    private Color _neutralFlagColor;

    [BoxGroup("Flag Color")] [SerializeField]
    private Color _playerFlagColor;

    private bool _isClaimed;

    private void Start() => _flagImage.color = _neutralFlagColor;

    public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
    {
        Player player = CellGrid.Instance.Players.First(player => player.PlayerNumber == 0);
        var humanPlayerUnitList = cellGrid.GetPlayerUnits(player);

        var unitsInRange = humanPlayerUnitList.Where(u =>
                u.Cell.GetDistance(UnitReference.Cell) <= _playerUnitRange && u is not LStructure)
            .ToList();

        if (unitsInRange.Count > 0)
            for (int i = 0; i < _structuresList.Count; i++)
                _structuresList[i].Claim();

        yield return 0;
    }

    public void Claim()
    {
        UnitReference.PlayerNumber = 0;
        _isClaimed = true;
        _flagImage.color = _playerFlagColor;
        OnClaimStructure?.Invoke();
    }
    
    public override void OnTurnStart(CellGrid cellGrid)
    {
        if (_isClaimed) return;
        StartCoroutine(Act(cellGrid, false));
    }

    public override void OnTurnEnd(CellGrid cellGrid)
    {
        if (_isClaimed) return;
        StartCoroutine(Act(cellGrid, false));
    }
}