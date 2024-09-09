using TbsFramework.Grid;
using TbsFramework.Units;
using UnityEngine;

public class Swordsman : LUnit, ISwordInfantry
{
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= GetClassCounterDamageFactor(lUnit);

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
    
    public override float GetClassCounterDamageFactor(LUnit enemyUnit)
    {
        if (enemyUnit.UnitClassCounter != null)
            return enemyUnit.UnitClassCounter.VSSwordInfantryCounter;
        return 1f;
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

        if (!IsEvaluating)
        {
            for (int i = 0; i < AttackSkillArray.Length; i++)
            {
                if (IsRetaliating && !AttackSkillArray[i].CanBeActivatedDuringEnemyTurn) continue;
                if (AttackSkillArray[i] is ParrySkill parrySkill)
                {
                    if (PlayerNumber == CellGrid.Instance.CurrentPlayerNumber) continue;
                    parrySkill.AggressorUnit = unitToAttack as LUnit;
                    totalFactorDamage += AttackSkillArray[i].GetDamageFactor();
                }
            }
        }

        int factoredDamage = totalFactorDamage > 0 ? baseDamage * (int)totalFactorDamage : baseDamage;
        return factoredDamage;
    }
}