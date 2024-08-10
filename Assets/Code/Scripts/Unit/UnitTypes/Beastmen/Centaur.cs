using TbsFramework.Units;
using UnityEngine;

public class Centaur : LUnit, IRanged
{
    private RapidShotSkill _rapidShotSkill;

    #region Properties

    public RapidShotSkill RapidShotSkill => _rapidShotSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _rapidShotSkill = GetComponent<RapidShotSkill>();
    }

    /*protected override void AttackActionPerformed(float actionCost)
    {
        if (_rapidShotSkill != null) _rapidShotSkill.AddAdditionalActionPoint();
        base.AttackActionPerformed(actionCost);
    }*/

    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= UnitClassCounter.VSRangedFactor;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }
}