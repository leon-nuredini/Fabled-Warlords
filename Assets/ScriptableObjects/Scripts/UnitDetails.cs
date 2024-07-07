using UnityEngine;

[CreateAssetMenu(fileName = "Units", menuName = "Information/InformationData", order = 0)]
public class UnitDetails : ScriptableObject
{
    [SerializeField] private string _unitName;
    [SerializeField] private string _faction;
    [SerializeField] private Sprite _icon;
    [TextArea] [SerializeField] private string _description;

    public void Initialize(string unitName, string faction, Sprite icon, string description)
    {
        _unitName = unitName;
        _faction = faction;
        _icon = icon;
        _description = description;
    }

    public string UnitName => _unitName;
    public string Faction => _faction;
    public Sprite Icon => _icon;
    public string Description => _description;
}