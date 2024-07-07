using NaughtyAttributes;
using UnityEngine;

public class ShieldwallSkill : MonoBehaviour, IDefendSkill
{
    private LUnit _lUnit;
    private Collider2D _collider2D;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    [BoxGroup("Defence Amount")] [SerializeField] [Range(1, 30)]
    private int _shieldwallDefenceAmount = 10;

    [BoxGroup("Box Cast")] [SerializeField]
    private Vector2 _boxCastSize = new Vector2(3f, 3f);

    [BoxGroup("Box Cast")] [SerializeField]
    private LayerMask _unitLayerMask;

    [SerializeField] private Collider2D[] _colliderArray;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;
    public int DefenceAmount => _shieldwallDefenceAmount;
    public LUnit LUnit => _lUnit;

    private void Awake()
    {
        _collider2D = GetComponent<Collider2D>();
        _lUnit = GetComponent<LUnit>();
    }

    public int GetDefenceAmount()
    {
        _colliderArray = Physics2D.OverlapBoxAll(transform.localPosition, _boxCastSize, 0f, _unitLayerMask);

        for (int i = 0; i < _colliderArray.Length; i++)
        {
            if (_colliderArray[i] == _collider2D) continue;
            if (_colliderArray[i].TryGetComponent(out ShieldwallSkill shieldwallSkill))
            {
                if (shieldwallSkill.LUnit.PlayerNumber == _lUnit.PlayerNumber)
                    return _shieldwallDefenceAmount;
            }
        }

        return 0;
    }

    /*private void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.localPosition, _boxCastSize);
    }*/
}