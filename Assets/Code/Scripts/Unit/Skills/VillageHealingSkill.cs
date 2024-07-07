using System.Collections;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class VillageHealingSkill : Ability, IAOEHealingSkill
{
    public event Action<Transform[]> OnHeal;

    [SerializeField] private string _skillName;
    [SerializeField] private string _skillDescription;

    public string SkillName => _skillName;
    public string SkillDescription => _skillDescription;

    [SerializeField] private Buff _aoeHealingBuff;
    public int Range = 1;

    public Buff AoeHealingBuff => _aoeHealingBuff;

    [SerializeField] private Unit[] unitsInRangeArray;

    public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
    {
        var myUnits = cellGrid.GetCurrentPlayerUnits();
        var unitsInRange = myUnits.Where(u =>
            u.Cell.GetDistance(UnitReference.Cell) <= Range && u is not LStructure && u.HitPoints < u.TotalHitPoints);

        unitsInRangeArray = unitsInRange as Unit[] ?? unitsInRange.ToArray();
        if (unitsInRangeArray.Length > 0)
        {
            int randomIndex = Random.Range(0, unitsInRangeArray.Length);
            unitsInRangeArray[randomIndex].AddBuff(AoeHealingBuff);
        }

        Transform[] vfxSpawnTransformArray = new Transform[unitsInRangeArray.Length];
        for (int i = 0; i < unitsInRangeArray.Length; i++)
            vfxSpawnTransformArray[i] = unitsInRangeArray[i].transform;

        OnHeal?.Invoke(vfxSpawnTransformArray);
        yield return 0;
    }

    public override void OnTurnStart(CellGrid cellGrid) => StartCoroutine(Act(cellGrid, false));
}