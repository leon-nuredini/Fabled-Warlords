using TbsFramework.Units;

public class Pikeman : LUnit
{
    protected override int CalculateDamage(AttackAction baseVal, Unit unitToAttack)
    {
        float totalFactorDamage = 0;
        int   baseDamage        = baseVal.Damage;
        for (int i = 0; i < AttackSkillArray.Length; i++)
        {
            if (!AttackSkillArray[i].CanBeActivatedDuringEnemyTurn) continue;
            if (unitToAttack is ILarge && AttackSkillArray[i] is AntiLargeSkill antiLargeSkill)
            {
                antiLargeSkill.AggressorUnit =  unitToAttack as LUnit;
                totalFactorDamage            += AttackSkillArray[i].GetDamageFactor();
            }
        }

        int factoredDamage = totalFactorDamage > 0 ? baseDamage * (int) totalFactorDamage : baseDamage;
        return factoredDamage;
    }
}
