using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;

public class Griffin : LUnit, IMonster
{
    private SoarSkill _soarSkill;

    #region Properties

    public SoarSkill SoarSkill => _soarSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _soarSkill = GetComponent<SoarSkill>();
    }

    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= GetClassCounterDamageFactor(lUnit);

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }

    public override float GetClassCounterDamageFactor(LUnit enemyUnit)
    {
        if (enemyUnit.UnitClassCounter != null)
            return enemyUnit.UnitClassCounter.VSMonsterFactor;
        return 1f;
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
                if (pathCost <= MovementPoints)
                {
                    availableDestinations.Add(cell);
                }
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
    
    protected override void UpdateMovementPoints(IList<Cell> path)
    {
        var totalMovementCost = path.Sum(h => 1);
        MovementPoints -= totalMovementCost;
    }
}