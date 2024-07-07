public class RecruitUnitDataPresenter : BaseUnitDataPresenter
{
    private UIRecruitment _uiRecruitment;

    private void Awake() => _uiRecruitment = GetComponent<UIRecruitment>();
    private void OnEnable() => _uiRecruitment.OnUpdateUnitDetails += UpdateUnitData;
    private void OnDisable() => _uiRecruitment.OnUpdateUnitDetails -= UpdateUnitData;
}
