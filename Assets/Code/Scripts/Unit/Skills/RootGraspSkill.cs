using UnityEngine;

public class RootGraspSkill : MonoBehaviour, IStatusEffectSkill
{
    [SerializeField] private string _skillName = "Root Grasp";

    [SerializeField] private string _skillDescription =
        "Entagle an enemy, preventing them from moving and reducing their attack power for one turn.";

    [SerializeField] private int _durationInTurns = 1;

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;
    public int    DurationInTurns  => _durationInTurns;
}