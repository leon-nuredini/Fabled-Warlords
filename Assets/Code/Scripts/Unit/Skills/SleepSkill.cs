using UnityEngine;

public class SleepSkill : MonoBehaviour, IStatusEffectSkill
{
    [SerializeField] private string _skillName = "Sleep";

    [SerializeField] private string _skillDescription =
        "Has a chance to put an enemy to sleep for one turn.";

    [SerializeField] private int _durationInTurns = 1;
    [Range(0f, 100f)] [SerializeField] private float _procChance = 75;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
    public int DurationInTurns => _durationInTurns;
    public float ProcChance => _procChance;
}