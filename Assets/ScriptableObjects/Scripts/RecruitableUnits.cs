using UnityEngine;

[CreateAssetMenu(fileName = "RecruitableUnits", menuName = "LevelSettings/RecrutableUnits", order = 0)]
public class RecruitableUnits : ScriptableObject
{
    [SerializeField] private bool _canRecruitSpearman;
    [SerializeField] private bool _canRecruitArcher;
    [SerializeField] private bool _canRecruitSwordsman;
    [SerializeField] private bool _canRecruitWizard;
    [SerializeField] private bool _canRecruitBerserker;
    [SerializeField] private bool _canRecruitLanceKnight;
    [SerializeField] private bool _canRecruitAssassin;
    [SerializeField] private bool _canRecruitAxeKnight;

    public bool CanRecruitSpearman => _canRecruitSpearman;
    public bool CanRecruitArcher => _canRecruitArcher;
    public bool CanRecruitSwordsman => _canRecruitSwordsman;
    public bool CanRecruitWizard => _canRecruitWizard;
    public bool CanRecruitBerserker => _canRecruitBerserker;
    public bool CanRecruitLanceKnight => _canRecruitLanceKnight;
    public bool CanRecruitAssassin => _canRecruitAssassin;
    public bool CanRecruitAxeKnight => _canRecruitAxeKnight;
}
