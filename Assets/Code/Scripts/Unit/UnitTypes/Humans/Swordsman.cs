using TbsFramework.Grid;
using TbsFramework.Units;

public class Swordsman : LUnit
{
    protected override int CalculateDamage(AttackAction baseVal, Unit unitToAttack)
    {
        float totalFactorDamage = 0;
        int   baseDamage        = baseVal.Damage;
        for (int i = 0; i < AttackSkillArray.Length; i++)
        {
            if (IsRetaliating && !AttackSkillArray[i].CanBeActivatedDuringEnemyTurn) continue;
            if (AttackSkillArray[i] is ParrySkill parrySkill)
            {
                if (PlayerNumber == CellGrid.Instance.CurrentPlayerNumber) continue;
                parrySkill.AggressorUnit =  unitToAttack as LUnit;
                totalFactorDamage        += AttackSkillArray[i].GetDamageFactor();
            }
        }

        int factoredDamage = totalFactorDamage > 0 ? baseDamage * (int) totalFactorDamage : baseDamage;
        return factoredDamage;
    }
}
