using UnityEngine;
using System;
using Random = UnityEngine.Random;
using NaughtyAttributes;
using Lean.Pool;

public class ParrySkill : MonoBehaviour, IAttackSkill, ISpawnableEffect
{
    [SerializeField] private string _skillName = "Parry";

    [SerializeField] private string _skillDescription =
        "Has a chance to block and incoming melee attack and counterattack dealing critical damage.";

    [SerializeField] [Range(0, 100)] private int     _parryChance          = 20;
    [SerializeField] [Range(0, 100)] private int     _parryDamageFactor    = 2;

    [BoxGroup("Effect")][SerializeField] private GameObject _effect;

    private LUnit _lUnit;
    private LUnit _aggressorUnit;

    #region Properties

    public bool CanBeActivatedDuringEnemyTurn { get; set; } = true;

    public string SkillName => _skillName;

    public string SkillDescription => _skillDescription;

    public LUnit AggressorUnit { get => _aggressorUnit; set => _aggressorUnit = value; }

    public GameObject Effect => _effect;

    #endregion
    
    private void Awake() { _lUnit = GetComponent<LUnit>(); }

    public int GetDamageFactor()
    {
        int randomValue = Random.Range(1, 100);
        bool isParrySuccessfull = randomValue <= _parryChance;

        if (isParrySuccessfull)
        {
            if (ParryTextSpawner.Instance != null)
                ParryTextSpawner.Instance.SpawnTextGameObject(_lUnit, transform.position);
            SpawnEffect(transform);
            return _parryDamageFactor;
        }

        return 0;
    }

    public void SpawnEffect(Transform targetTransform)
    {
        if (Effect == null) return;
        LeanPool.Spawn(Effect, targetTransform.localPosition, Effect.transform.localRotation);
    }
}