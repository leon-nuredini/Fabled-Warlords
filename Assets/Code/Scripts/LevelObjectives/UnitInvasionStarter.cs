using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using UnityEngine;

public class UnitInvasionStarter : MonoBehaviour
{
    public static event Action<int> OnAnyTurnUntilInvasionUpdated;

    private List<StationaryGroupSkill> _stationaryUnitsList = new List<StationaryGroupSkill>();
    [SerializeField] private List<UnAttackableAbility> _unAttackableStructures;

    [SerializeField] private int _invasionTurn = 15;

    private void Awake()
    {
        _stationaryUnitsList = FindObjectsOfType<StationaryGroupSkill>().ToList();
        _unAttackableStructures = FindObjectsOfType<UnAttackableAbility>().ToList();
    }

    private void OnEnable()
    {
        CellGrid.Instance.OnTurnNumberIncreased += TurnPassed;
    }

    private void OnDisable()
    {
        CellGrid.Instance.OnTurnNumberIncreased -= TurnPassed;
    }

    private void TurnPassed(int turnNumber)
    {
        if (turnNumber == _invasionTurn)
        {
            for (int i = 0; i < _stationaryUnitsList.Count; i++)
                _stationaryUnitsList[i].Alert();
            
            for (int i = 0; i < _unAttackableStructures.Count; i++)
            {
                if (_unAttackableStructures[i] != null)
                    _unAttackableStructures[i].MakeAttackable();
            }
        }

        OnAnyTurnUntilInvasionUpdated?.Invoke(_invasionTurn - turnNumber + 1);
    }
}