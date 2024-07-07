using System.Collections;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;
using System;
using System.Collections.Generic;

public class AOEHealingSkill : Ability, IAOEHealingSkill
{
    public event Action<Transform[]> OnHeal;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    [SerializeField] private Buff _aoeHealingBuff;
    public int Range = 1;
    public bool ApplyToSelf;

    public Buff AoeHealingBuff => _aoeHealingBuff;

    public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
    {
        var myUnits = cellGrid.GetCurrentPlayerUnits();
        var unitsInRange = myUnits.Where(u =>
            u.Cell.GetDistance(UnitReference.Cell) <= Range && u is not LStructure && u.HitPoints < u.TotalHitPoints);

        List<Transform> vfxSpawnTransformList = new List<Transform>();
        foreach (var unit in unitsInRange)
        {
            if (unit.Equals(UnitReference) && !ApplyToSelf)
                continue;
            unit.AddBuff(AoeHealingBuff);
            vfxSpawnTransformList.Add(unit.transform);
        }

        OnHeal?.Invoke(vfxSpawnTransformList.ToArray());
        yield return 0;
    }

    public override void OnTurnStart(CellGrid cellGrid) => StartCoroutine(Act(cellGrid, false));
}