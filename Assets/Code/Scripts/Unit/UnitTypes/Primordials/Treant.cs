using TbsFramework.Units;
using UnityEngine;

public class Treant : LUnit, IMonster
{
    private RootGraspSkill _rootGraspSkill;

    #region Properties

    public RootGraspSkill RootGraspSkill => _rootGraspSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _rootGraspSkill = GetComponent<RootGraspSkill>();
    }

    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= lUnit.UnitClassCounter.VSMonsterFactor;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }

    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (RootGraspSkill == null) return;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(RootGraspSkill.DurationInTurns);
    }
}