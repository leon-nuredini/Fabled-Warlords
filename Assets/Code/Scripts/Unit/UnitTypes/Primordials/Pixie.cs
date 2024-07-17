using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;

public class Pixie : LUnit
{
    private SleepSkill _sleepSkill;
    private SoarSkill _soarSkill;
    
    #region Properties

    public SleepSkill SleepSkill => _sleepSkill;
    public SoarSkill SoarSkill => _soarSkill;

    #endregion
    
    public override void InitProperties()
    {
        base.InitProperties();
        _sleepSkill = GetComponent<SleepSkill>();
        _soarSkill = GetComponent<SoarSkill>();
    }
    
    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (SleepSkill == null) return;
        float randomChance = UnityEngine.Random.Range(0f, 100f);
        if (randomChance < SleepSkill.ProcChance) return;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(SleepSkill.DurationInTurns);
    }
    
    public override HashSet<Cell> GetAvailableDestinations(List<Cell> cells)
    {
        if (_soarSkill == null) return base.GetAvailableDestinations(cells);

        if (CachedPaths == null) CachePaths(cells);

        var availableDestinations = new HashSet<Cell>();
        foreach (var cell in cells)
        {
            if (CachedPaths.TryGetValue(cell, out var path))
            {
                var pathCost = path.Sum(c => 1);
                if (pathCost <= MovementPoints) { availableDestinations.Add(cell); }
            }
        }

        return availableDestinations;
    }

    protected override Dictionary<Cell, Dictionary<Cell, float>> GetGraphEdges(List<Cell> cells)
    {
        if (_soarSkill == null) return base.GetGraphEdges(cells);

        Dictionary<Cell, Dictionary<Cell, float>> ret = new Dictionary<Cell, Dictionary<Cell, float>>();
        foreach (var cell in cells)
        {
            if (IsCellTraversable(cell) || cell == Cell)
            {
                ret[cell] = new Dictionary<Cell, float>();
                foreach (var neighbour in cell.GetNeighbours(cells))
                {
                    if (IsCellTraversable(neighbour) || IsCellMovableTo(neighbour))
                        ret[cell][neighbour] = 1;
                }
            }
        }

        return ret;
    }
}
