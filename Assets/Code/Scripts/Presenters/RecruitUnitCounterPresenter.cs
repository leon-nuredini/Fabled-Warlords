using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class RecruitUnitCounterPresenter : UnitCounterPresenter
{
    private UIRecruitment _uiRecruitment;

    private void Awake() => _uiRecruitment = GetComponent<UIRecruitment>();

    private void OnEnable()
    {
        if (_uiRecruitment == null) return;
        _uiRecruitment.OnUpdateUnitDetails += UpdateCounterImages;
    }

    private void OnDisable()
    {
        if (_uiRecruitment == null) return;
        _uiRecruitment.OnUpdateUnitDetails -= UpdateCounterImages;
    }
}