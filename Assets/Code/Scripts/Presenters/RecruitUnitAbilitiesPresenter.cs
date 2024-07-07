public class RecruitUnitAbilitiesPresenter : BaseUnitAbilitiesPresenter
{
    private UIRecruitment _uiRecruitment;

    private void Awake() => _uiRecruitment = GetComponent<UIRecruitment>();
    private void OnEnable() => _uiRecruitment.OnUpdateUnitDetails += UpdateUnitAbilities;
    private void OnDisable() => _uiRecruitment.OnUpdateUnitDetails -= UpdateUnitAbilities;
}