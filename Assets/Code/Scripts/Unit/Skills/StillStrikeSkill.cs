using NaughtyAttributes;
using UnityEngine;

public class StillStrikeSkill : MonoBehaviour, IAttackSkill
{
    private LUnit _lUnit;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    [BoxGroup("Damage Multiplier")] [SerializeField] [Range(1, 10)]
    private int _stillStrikeDamageFactor = 2;

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;

    [field: SerializeField] public bool CanBeActivatedDuringEnemyTurn { get; set; } = false;

    private void Awake() => _lUnit = GetComponent<LUnit>();

    public int GetDamageFactor()
    {
        if (_lUnit.MovementPoints < _lUnit.TotalMovementPoints) return 0;
        return _stillStrikeDamageFactor;
    }
}