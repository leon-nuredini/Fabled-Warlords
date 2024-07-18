using TbsFramework.Units;
using UnityEngine;

public class Deer : LUnit, IMonster
{
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is ISpearInfantry) newDamage *= 1.5f;
        if (other is IRanged) newDamage *= 1.25f;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }

    protected override int CalculateDamage(AttackAction baseVal, Unit unitToAttack)
    {
        float totalFactorDamage = 0;
        int baseDamage = baseVal.Damage;

        if (StatusEffectsController.IsStatusApplied<Weaken>())
        {
            float weakenedFactor = StatusEffectsController.GetStatus<Weaken>().weakenFactor;
            baseDamage = Mathf.RoundToInt(baseDamage * weakenedFactor);
        }

        for (int i = 0; i < AttackSkillArray.Length; i++)
        {
            if (IsRetaliating && !AttackSkillArray[i].CanBeActivatedDuringEnemyTurn) continue;
            if (AttackSkillArray[i] is ChargeSkill chargeSkill)
                totalFactorDamage += AttackSkillArray[i].GetDamageFactor();
        }

        int factoredDamage = totalFactorDamage > 0 ? baseDamage * (int)totalFactorDamage : baseDamage;
        return factoredDamage;
    }
}