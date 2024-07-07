using System;
using TbsFramework.Units;
using UnityEngine;

[CreateAssetMenu(fileName = "Abilities", menuName = "Abilities/HealingBuff", order = 0)]
public class HealingBuff : Buff
{
    public static Action<Vector3, string> OnAnyHealUnit;
    
    public int HealingFactor;

    public HealingBuff(int duration, int healingFactor)
    {
        HealingFactor = healingFactor;
        Duration = duration;
    }

    public override void Apply(Unit unit)
    {
        AddHitPoints(unit, HealingFactor);
    }

    public override void Undo(Unit unit)
    {
        //Note that healing buff has empty Undo method implementation.
    }

    private void AddHitPoints(Unit unit, int amount)
    {
        if (unit is LUnit lUnit)
        {
            lUnit.HitPoints = Mathf.Clamp(unit.HitPoints + amount, 0, unit.TotalHitPoints);
            HealTextSpawner.Instance.SpawnTextGameObject(lUnit.transform.position, amount.ToString());
        }
    }
}