using UnityEngine;

[CreateAssetMenu(fileName = "UnitStats", menuName = "UnitStats/Structure", order = 0)]
public class StructureStats : UnitStats
{
    [SerializeField] private int _income;

    public int Income => _income;
}