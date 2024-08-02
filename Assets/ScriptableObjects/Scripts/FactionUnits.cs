using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FactionUnits", menuName = "FactionUnits/FactionUnits", order = 0)]
public class FactionUnits : ScriptableObject
{
    [SerializeField] private List<GameObject> _unitList = new List<GameObject>();
    
    public List<GameObject> UnitList => _unitList;
}
