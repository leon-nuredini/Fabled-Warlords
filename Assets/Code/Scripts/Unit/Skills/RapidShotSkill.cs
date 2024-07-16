using System;
using UnityEngine;

public class RapidShotSkill : MonoBehaviour, ISkill
{
    private LUnit _lUnit;

    [SerializeField] private string _skillName = "Rapid Shot";

    [SerializeField] private string _skillDescription =
        "Can shoot two arrows in quick succession, dealing double the damage of a regular attack.";

    private bool _isActionPointAdded;
    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
    }

    private void OnEnable() => _lUnit.OnTurnEndUnitReset += ResetActionPointAdded;
    private void OnDisable() => _lUnit.OnTurnEndUnitReset -= ResetActionPointAdded;

    public void AddAdditionalActionPoint()
    {
        if (_isActionPointAdded) return;
        _lUnit.ActionPoints++;
        _isActionPointAdded = true;
    }

    private void ResetActionPointAdded() => _isActionPointAdded = false;
}