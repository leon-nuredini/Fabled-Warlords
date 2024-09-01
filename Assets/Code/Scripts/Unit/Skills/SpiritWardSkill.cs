using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiritWardSkill : MonoBehaviour, ISkill
{
    [SerializeField] private string _skillName = "Spirit Ward";
    [SerializeField] private string _skillDescription = "Grants immunity to all status effects.";

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
}
