using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;

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