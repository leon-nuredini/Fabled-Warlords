using NaughtyAttributes;
using UnityEngine;

public class BackstabSkill : MonoBehaviour, IAttackSkill
{
    private LUnit _lUnit;
    private LUnit _unitToAttack;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    [BoxGroup("Damage Multiplier")] [SerializeField] [Range(1, 10)]
    private int _backstabDamageFactor = 3;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    public LUnit UnitToAttack
    {
        get => _unitToAttack;
        set => _unitToAttack = value;
    }

    [field: SerializeField] public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    public int GetDamageFactor()
    {
        int factor = 0;
        if (_unitToAttack is LStructure) return factor;

        if (Mathf.Approximately(_lUnit.transform.localPosition.y, _unitToAttack.transform.localPosition.y) &&
            _lUnit.CurrentUnitDirection == _unitToAttack.CurrentUnitDirection)
        {
            factor = _backstabDamageFactor;
            if (BackstabTextSpawner.Instance != null)
                BackstabTextSpawner.Instance.SpawnTextGameObject(_lUnit, transform.position);
        }


        _unitToAttack = null;
        return factor;
    }
}