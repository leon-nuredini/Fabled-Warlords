using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FactionUnits", menuName = "FactionUnits/FactionUnits", order = 0)]
public class FactionUnits : ScriptableObject
{
    [SerializeField] private FactionType _factionType;
    
    [SerializeField] private List<RecruitableUnit> _recruitableUnitList = new List<RecruitableUnit>();

    private List<GameObject> _unitList;

    #region Properties

    public List<GameObject> UnitList
    {
        get
        {
            if (_unitList.Count == 0)
            {
                for (int i = 0; i < _recruitableUnitList.Count; i++)
                    if (_recruitableUnitList[i].CanRecruit)
                        _unitList.Add(_recruitableUnitList[i].Unit);
            }

            return _unitList;
        }
    }

    public FactionType FactionType => _factionType;

    public List<RecruitableUnit> RecruitableUnitList => _recruitableUnitList;

    #endregion
}

[Serializable]
public class RecruitableUnit
{
    [SerializeField] private GameObject _unit;
    [SerializeField] private LUnit _lUnit;
    [SerializeField] private bool _canRecruit = true;

    public GameObject Unit => _unit;
    public LUnit LUnit => _lUnit;
    public bool CanRecruit => _canRecruit;
}