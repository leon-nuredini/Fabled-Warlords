using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;

public class Pixie : LUnit, IMage
{
    private StunSkill _sunSkill;
    private SoarSkill _soarSkill;

    #region Properties

    public StunSkill StunSkill => _sunSkill;
    public SoarSkill SoarSkill => _soarSkill;

    #endregion

    public override void InitProperties()
    {
        base.InitProperties();
        _sunSkill = GetComponent<StunSkill>();
        _soarSkill = GetComponent<SoarSkill>();
    }

    protected override int Defend(Unit other, int damage)
    {
        float newDamage = damage;
        if (other is LUnit lUnit && lUnit.UnitClassCounter != null)
            newDamage *= lUnit.UnitClassCounter.VSMageFactor;

        return base.Defend(other, Mathf.RoundToInt(newDamage));
    }

    protected override void ApplyDebuffsToEnemy(LUnit enemyUnit, bool isEnemyTurn = false)
    {
        if (enemyUnit is LStructure) return;
        if (isEnemyTurn) return;
        if (StunSkill == null) return;
        if (enemyUnit.HitPoints <= 0) return;
        float randomChance = Random.Range(0f, 100f);
        if (randomChance > StunSkill.ProcChance) return;
        enemyUnit.StatusEffectsController.ApplyStatusEffect<Stun>(StunSkill.DurationInTurns);
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