using TbsFramework.Units.Abilities;
using UnityEngine;

public class IncomeGenerationAbility : Ability, ISkill
{
    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;
    
    private LUnit _lUnit;
    private EconomyController _economyController;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
}