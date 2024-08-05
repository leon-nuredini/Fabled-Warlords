using TbsFramework.Grid;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class IncomeGenerationAbility : Ability, ISkill
{
    [SerializeField] private int _incomeAmount;
    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    private string _originalDescription;

    private LUnit _lUnit;
    private EconomyController _economyController;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    #region Properties

    public int IncomeAmount
    {
        get => _incomeAmount;
        set
        {
            _incomeAmount = value;
            UpdateDescriptionText();
        }
    }

    #endregion

    private void Awake()
    {
        _lUnit = GetComponent<LUnit>();
        _economyController = GetComponent<EconomyController>();
        _originalDescription = _skillDescription;
        UpdateDescriptionText();
    }

    public override void OnTurnStart(CellGrid cellGrid)
    {
        if (_economyController != null)
            _economyController.UpdateCurrentWealth(null, _lUnit.PlayerNumber, _incomeAmount);
    }

    private void UpdateDescriptionText() =>
        _skillDescription = _originalDescription.Replace("{X}", $"<b>{_incomeAmount}</b>");
}