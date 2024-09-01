using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ClaimBaseObjective : MonoBehaviour
{
    public static event Action OnAnyCompleteClaimBaseObjective;

    [SerializeField] private int _turnsToSurvive = 20;
    [SerializeField] private List<UnAttackableAbility> _unAttackableStructures;
    [SerializeField] private List<StationaryGroupSkill> _stationaryGroupUnits;
    [SerializeField] private List<ClaimAbility> _claimableStructures;

    private bool _isObjectiveComplete;

    private void Awake()
    {
        _unAttackableStructures = FindObjectsOfType<UnAttackableAbility>().ToList();
        _stationaryGroupUnits = FindObjectsOfType<StationaryGroupSkill>().ToList();
        _claimableStructures = FindObjectsOfType<ClaimAbility>().ToList();
    }

    private void OnEnable()
    {
        for (int i = 0; i < _claimableStructures.Count; i++)
            _claimableStructures[i].OnClaimStructure += CompleteFirstObjective;
    }

    private void CompleteFirstObjective()
    {
        if (_isObjectiveComplete) return;

        for (int i = 0; i < _unAttackableStructures.Count; i++)
        {
            if (_unAttackableStructures[i] != null)
                _unAttackableStructures[i].MakeAttackable();
        }

        for (int i = 0; i < _stationaryGroupUnits.Count; i++)
        {
            if (_stationaryGroupUnits[i] != null)
                _stationaryGroupUnits[i].Alert();
        }

        OnAnyCompleteClaimBaseObjective?.Invoke();
        _isObjectiveComplete = true;
    }
}