using NaughtyAttributes;
using UnityEngine;

public class AlliedArmamentSkill : MonoBehaviour, IAttackSkill
{
    private LUnit _lUnit;
    private Collider2D _collider2D;
    
    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;
    
    [BoxGroup("Attack Amount")] [SerializeField] [Range(1, 30)]
    private int _attackPowerFactor = 2;

    [BoxGroup("Box Cast")] [SerializeField]
    private Vector2 _boxCastSize = new Vector2(3f, 3f);

    [BoxGroup("Box Cast")] [SerializeField]
    private LayerMask _unitLayerMask;
    [SerializeField] private Collider2D[] _colliderArray;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
    public int AttackPowerFactor => _attackPowerFactor;
    public LUnit LUnit => _lUnit;
    
    [field: SerializeField] public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;
    
    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _lUnit = GetComponent<LUnit>();
    }
    
    public int GetDamageFactor()
    {
        _colliderArray = Physics2D.OverlapBoxAll(transform.localPosition, _boxCastSize, 0f, _unitLayerMask);
        for (int i = 0; i < _colliderArray.Length; i++)
        {
            if (_colliderArray[i] == _collider2D) continue;
            if (_colliderArray[i].TryGetComponent(out AlliedArmamentSkill alliedArmamentSkill))
            {
                if (alliedArmamentSkill.LUnit.PlayerNumber == _lUnit.PlayerNumber)
                    return _attackPowerFactor;
            }
        }

        return 0;
    }
}
