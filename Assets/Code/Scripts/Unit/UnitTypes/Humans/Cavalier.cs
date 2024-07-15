using TbsFramework.Units;
using UnityEngine;

public class Cavalier : LUnit, IMounted
{
    protected override int CalculateDamage(AttackAction baseVal, Unit unitToAttack)
    {
        float totalFactorDamage = 0;
        int   baseDamage        = baseVal.Damage;
        
        if (StatusEffectsController.IsWeakenApplied()) baseDamage = Mathf.RoundToInt(baseDamage / 1.5f);
        
        for (int i = 0; i < AttackSkillArray.Length; i++)
        {
            if (IsRetaliating && !AttackSkillArray[i].CanBeActivatedDuringEnemyTurn) continue;
            if (AttackSkillArray[i] is ChargeSkill chargeSkill)
                totalFactorDamage += AttackSkillArray[i].GetDamageFactor();
        }

        int factoredDamage = totalFactorDamage > 0 ? baseDamage * (int) totalFactorDamage : baseDamage;
        return factoredDamage;
    }
}