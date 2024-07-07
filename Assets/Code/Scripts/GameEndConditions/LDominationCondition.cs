using System.Collections.Generic;
using System.Linq;
using TbsFramework.Grid;
using TbsFramework.Grid.GameResolvers;
using UnityEngine;

public class LDominationCondition : GameEndCondition
{
    [SerializeField] List<LUnit> lUnitList = new List<LUnit>();
    public override GameResult CheckCondition(CellGrid cellGrid)
    {
        lUnitList.Clear();
        for (int i = 0; i < cellGrid.Units.Count; i++)
        {
            if (cellGrid.Units[i] is LUnit lUnit)
            {
                if (lUnit.UnitFaction != UnitFaction.None)
                    lUnitList.Add(lUnit);
            }
        }

        var playersAlive = lUnitList.Select(u => u.PlayerNumber).Distinct().ToList();
        if (playersAlive.Count == 1)
        {
            var playersDead = cellGrid.Players.Where(p => p.PlayerNumber != playersAlive[0])
                .Select(p => p.PlayerNumber)
                .ToList();

            return new GameResult(true, playersAlive, playersDead);
        }

        return new GameResult(false, null, null);
    }
}