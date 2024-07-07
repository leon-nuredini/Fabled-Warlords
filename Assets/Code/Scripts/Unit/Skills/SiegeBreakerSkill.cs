using NaughtyAttributes;
using UnityEngine;

public class SiegeBreakerSkill : MonoBehaviour, IAttackSkill
{
    private LStructure _structureToAttack;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    #region Properties

    public LStructure StructureToAttack
    {
        get => _structureToAttack;
        set => _structureToAttack = value;
    }
    
    [field: SerializeField] public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;

    #endregion

    [BoxGroup("Damage Multiplier")] [SerializeField] [Range(1, 10)]
    private int _structureDamageFactor = 2;

    public int GetDamageFactor()
    {
        int factor = 0;
        
        if (StructureToAttack != null)
            factor = _structureDamageFactor;

        StructureToAttack = null;
        return factor;
    }
}