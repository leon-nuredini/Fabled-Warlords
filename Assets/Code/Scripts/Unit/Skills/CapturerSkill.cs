using UnityEngine;

public class CapturerSkill : MonoBehaviour, ISkill
{
    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
}