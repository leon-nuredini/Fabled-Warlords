using UnityEngine;
using UnityEngine.UI;

public class UITopTutorialLevelPresenter : MonoBehaviour
{
    [SerializeField] private Button _endTurnButton;
    [SerializeField] private Button _recruitButton;

    private UIRecruitment              _uiRecruitment;
    private UITop                      _uiTop;
    private ToggleUnitDetailsPresenter _toggleUnitDetailsPresenter;

    private void Awake()
    {
        _endTurnButton.gameObject.SetActive(false);
        _recruitButton.gameObject.SetActive(false);

        _uiTop                      = GetComponent<UITop>();
        _toggleUnitDetailsPresenter = GetComponent<ToggleUnitDetailsPresenter>();
        _uiRecruitment              = FindObjectOfType<UIRecruitment>();
    }

    private void Start()
    {
        _uiTop.AllowEndTurn                                  = false;
        _toggleUnitDetailsPresenter.AllowTogglingUnitDetails = false;
        if (_uiRecruitment == null) return;
        _uiRecruitment.AllowRecruitment = false;
    }

    private void OnEnable()
    {
        UITutorial.OnAnyAllowEndingTurn   += EnableEndingTurn;
        UITutorial.OnAnyAllowRecruitment  += AllowRecruitment;
        UITutorial.OnAnyToggleUnitDetails += AllowTogglingUnitDetails;
    }

    private void OnDisable()
    {
        UITutorial.OnAnyAllowEndingTurn   -= EnableEndingTurn;
        UITutorial.OnAnyAllowRecruitment  -= AllowRecruitment;
        UITutorial.OnAnyToggleUnitDetails -= AllowTogglingUnitDetails;
    }

    private void EnableEndingTurn()
    {
        _uiTop.AllowEndTurn = true;
        _endTurnButton.gameObject.SetActive(true);
    }

    private void AllowRecruitment()
    {
        if (_uiRecruitment == null) return;
        _recruitButton.gameObject.SetActive(true);
        _uiRecruitment.AllowRecruitment = true;
    }

    private void AllowTogglingUnitDetails() => _toggleUnitDetailsPresenter.AllowTogglingUnitDetails = true;
}