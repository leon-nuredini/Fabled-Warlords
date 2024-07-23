using UnityEngine;

public class OverpowerSkill : MonoBehaviour, IStatusEffectSkill
{
    [SerializeField] private string _skillName = "Overpower";

    [SerializeField] private string _skillDescription =
        "Prevents the enemy from moving or attacking for one turn after a successful hit.";

    [SerializeField] private int _durationInTurns = 1;
    [Range(0f, 100f)][SerializeField] private float _procChance = 50f;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
    public int DurationInTurns => _durationInTurns;
    public float ProcChance => _procChance;
}