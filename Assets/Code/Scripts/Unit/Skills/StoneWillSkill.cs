using UnityEngine;

public class StoneWillSkill : MonoBehaviour, IDefendSkill
{
    [SerializeField] private string _skillName = "Iron Will";

    [SerializeField] private string _skillDescription =
        "Temporarily increases its defense when attacking an enemy, reducing the damage taken from retaliatory attacks.";

    [Range(0f, .75f)] [SerializeField] private float _defenseFactor;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    public float DefenceAmount => _defenseFactor;

    public float GetDefenceAmount() => _defenseFactor;
}