using UnityEngine;

public class HexSkill : MonoBehaviour, IStatusEffectSkill
{
    [SerializeField] private string _skillName = "Hex";

    [SerializeField] private string _skillDescription =
        "Debuffs an enemy unit, reducing their attack power and defense for one turn.";

    [SerializeField] private int _durationInTurns = 1;

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;
    public int    DurationInTurns  => _durationInTurns;
}