using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class ParrySkill : MonoBehaviour, IAttackSkill
{
    public event Action<Transform[]> OnParry;

    [SerializeField] private string _skillName = "Parry";

    [SerializeField] private string _skillDescription =
        "Has a chance to block and incoming melee attack and counterattack dealing critical damage.";

    [SerializeField] [Range(0, 100)] private int     _parryChance          = 20;
    [SerializeField] [Range(0, 100)] private int     _parryDamageFactor    = 2;
    [SerializeField]                 private Vector3 _parryTextSpawnOffset = Vector3.zero;

    private LUnit _aggressorUnit;

    #region Properties

    public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;

    public string SkillName => _skillName;

    public string SkillDescription => _skillDescription;

    public LUnit AggressorUnit { get => _aggressorUnit; set => _aggressorUnit = value; }

    #endregion

    public int GetDamageFactor()
    {
        if (TryToParry())
        {
            ParryTextSpawner.Instance.SpawnTextGameObject(transform.position + _parryTextSpawnOffset);
            return _parryDamageFactor;
        }

        return 0;
    }

    private bool TryToParry()
    {
        int randomValue = Random.Range(1, 100);
        return randomValue <= _parryChance;
    }
}