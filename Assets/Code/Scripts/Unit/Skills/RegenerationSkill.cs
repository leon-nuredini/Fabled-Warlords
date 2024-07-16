using System.Collections;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;
using System;
using System.Collections.Generic;

public class RegenerationSkill : Ability, IAOEHealingSkill
{
    public event Action<Transform[]> OnHeal;

    [SerializeField] private string _skillName = "Regeneration";

    [SerializeField]
    private string _skillDescription = "Regenerates a portion of its health at the start of each turn.";

    public string SkillName        => _skillName;
    public string SkillDescription => _skillDescription;

    [SerializeField] private Buff _aoeHealingBuff;

    public Buff AoeHealingBuff => _aoeHealingBuff;

    public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
    {
        var myUnits = cellGrid.GetCurrentPlayerUnits();

        List<Transform> vfxSpawnTransformList = new List<Transform>();
        foreach (var unit in myUnits)
        {
            if (unit.Equals(UnitReference))
            {
                unit.AddBuff(AoeHealingBuff);
                vfxSpawnTransformList.Add(unit.transform);
            }
        }

        OnHeal?.Invoke(vfxSpawnTransformList.ToArray());
        yield return 0;
    }

    public override void OnTurnStart(CellGrid cellGrid) => StartCoroutine(Act(cellGrid, false));
}