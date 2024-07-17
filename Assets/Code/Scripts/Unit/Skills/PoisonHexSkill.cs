using UnityEngine;

public class PoisonHexSkill : MonoBehaviour, IStatusEffectSkill
{
    [SerializeField] private string _skillName = "Poison Hex";

    [SerializeField] private string _skillDescription =
        "Debuffs and poisons an enemy unit, reducing their attack power and defense for one turn.";

    [SerializeField] private int _durationInTurns = 1;

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;
    public int    DurationInTurns  => _durationInTurns;
}