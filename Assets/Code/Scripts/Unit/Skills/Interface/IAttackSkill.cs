using UnityEngine;

public interface IAttackSkill : ISkill
{
    bool CanBeActivatedDuringEnemyTurn { get; set; }
    int GetDamageFactor();
}
