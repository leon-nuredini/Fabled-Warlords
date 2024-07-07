using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIAbility : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _abilityNameText;
    [SerializeField] private TextMeshProUGUI _abilityDescriptionText;

    public TextMeshProUGUI AbilityNameText { get => _abilityNameText; private set => _abilityNameText = value; }

    public TextMeshProUGUI AbilityDescriptionText
    {
        get => _abilityDescriptionText;
        private set => _abilityDescriptionText = value;
    }

    public void UpdateNameAndDescription(ISkill skill)
    {
        AbilityNameText.text        = skill.SkillName;
        AbilityDescriptionText.text = skill.SkillDescription;
    }
}
