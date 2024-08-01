using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFaction", menuName = "PlayerFaction/Faction", order = 0)]
public class PlayerFaction : ScriptableObject
{
    [SerializeField] private FactionType _factionType = FactionType.None;
    [SerializeField] private GameObject _factionRecruitmentPanel;
    
    public FactionType FactionType => _factionType;
    public GameObject FactionRecruitmentPanel => _factionRecruitmentPanel;
}
