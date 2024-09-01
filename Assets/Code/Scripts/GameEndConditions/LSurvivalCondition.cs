using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TbsFramework.Grid;
using TbsFramework.Grid.GameResolvers;
using UnityEngine;

public class LSurvivalCondition : GameEndCondition
{
    public static event Action OnAnySurvivalConditionMet;

    [SerializeField] private bool _startCountingTurns;
    [SerializeField] private int _turnToSurvive = 3;

    private int _winTurn;

    public int TurnToSurvive
    {
        get => _turnToSurvive;
        set => _turnToSurvive = value;
    }

    private void OnEnable()
    {
        ClaimBaseObjective.OnAnyCompleteClaimBaseObjective += StartCountingTurns;
    }

    private void OnDisable()
    {
        ClaimBaseObjective.OnAnyCompleteClaimBaseObjective -= StartCountingTurns;
    }

    private void StartCountingTurns()
    {
        _startCountingTurns = true;
        _winTurn = CellGrid.Instance.TurnNumber + _turnToSurvive;
    }

    public override GameResult CheckCondition(CellGrid cellGrid)
    {
        if (!_startCountingTurns) return new GameResult(false, null, null);

        if (cellGrid.TurnNumber > _winTurn)
        {
            var playersAlive = new List<int>();
            playersAlive.Add(0);
            if (playersAlive.Count == 1)
            {
                var playersDead = new List<int>();
                playersDead.Add(1);

                OnAnySurvivalConditionMet?.Invoke();
                return new GameResult(true, playersAlive, playersDead);
            }
        }

        return new GameResult(false, null, null);
    }
}