using UnityEngine;

public class SoarSkill : MonoBehaviour, ISkill
{
    [SerializeField] private string _skillName = "Soar";

    [SerializeField] private string _skillDescription =
        "Ignores terrain movement penalites and moves to any unoccupied space within its range.";

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;
}