public class RecruitUnitStatsPresenter : BaseUnitStatsPresenter
{
    private UIRecruitment _uiRecruitment;

    private void Awake() => _uiRecruitment = GetComponent<UIRecruitment>();
    private void OnEnable() => _uiRecruitment.OnUpdateUnitDetails += UpdateUnitStats;
    private void OnDisable() => _uiRecruitment.OnUpdateUnitDetails -= UpdateUnitStats;
}