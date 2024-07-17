using UnityEngine;

public class ThunderStrikeSkill : MonoBehaviour, IStatusEffectSkill
{
    [SerializeField] private string _skillName = "Thunder Strike";

    [SerializeField] private string _skillDescription =
        "Unleashes a powerful bolt of lightning at an enemy, dealing high damage with a chance to stun the target for one turn.";

    [SerializeField] private int _durationInTurns = 1;
    [Range(0f, 100f)] [SerializeField] private float _procChance = 50f;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
    public int DurationInTurns => _durationInTurns;
    public float ProcChance => _procChance;
}