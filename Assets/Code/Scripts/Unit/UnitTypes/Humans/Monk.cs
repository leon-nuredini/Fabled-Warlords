using TbsFramework.Units;
using UnityEngine;

public class Monk : LUnit, IMage
{
    private HexSkill _hexSkill;
    
    #region Properties

    public HexSkill HexSkill => _hexSkill;
    
    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _hexSkill = GetComponent<HexSkill>();
    }
    
    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is IMonster) newDamage *= 1.25f;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }

    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (HexSkill == null) return;
        int extraTurn = isEnemyTurn ? 1 : 0;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Weaken>(_hexSkill.DurationInTurns + extraTurn);
    }
}
