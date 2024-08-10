using System.Collections.Generic;
using System.Linq;
using TbsFramework.Units;

namespace TbsFramework.Grid.GameResolvers
{
    public class DominationConditionWithoutStructures : GameEndCondition
    {
        public override GameResult CheckCondition(CellGrid cellGrid)
        {
            List<Unit> allUnits = cellGrid.Units;
            List<Unit> unitsOnly = new List<Unit>();
            
            for (int i = 0; i < allUnits.Count; i++)
            {
                if (allUnits[i] is LStructure)
                    continue;
                unitsOnly.Add(allUnits[i]);
            }
            
            var playersAlive = unitsOnly.Select(u => u.PlayerNumber).Distinct().ToList();
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
}