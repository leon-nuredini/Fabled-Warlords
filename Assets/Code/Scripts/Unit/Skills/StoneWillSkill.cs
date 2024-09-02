using UnityEngine;

public class StoneWillSkill : MonoBehaviour, IDefendSkill
{
    private LUnit _lUnit;
    
    [SerializeField] private string _skillName = "Iron Will";

    [SerializeField] private string _skillDescription =
        "Temporarily increases its defense when attacking an enemy, reducing the damage taken from retaliatory attacks.";

    [Range(0f, .75f)] [SerializeField] private float _defenseFactor;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
    public float DefenceAmount => _defenseFactor;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    public float GetDefenceAmount()
    {
        if (IronWillTextSpawner.Instance != null)
            IronWillTextSpawner.Instance.SpawnTextGameObject(_lUnit, transform.position);
        return _defenseFactor;
    }
}