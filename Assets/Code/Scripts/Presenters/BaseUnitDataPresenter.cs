using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BaseUnitDataPresenter : MonoBehaviour, IUnitPresenter
{
    [SerializeField] private GameObject _unitInformationPanel;
    [SerializeField] private Image _unitImage;
    [SerializeField] private TextMeshProUGUI _unitNameText;

    #region Properties

    protected GameObject UnitInformationPanel
    {
        get => _unitInformationPanel;
        set => _unitInformationPanel = value;
    }

    protected Image UnitImage
    {
        get => _unitImage;
        set => _unitImage = value;
    }

    #endregion

    protected virtual void UpdateUnitData(LUnit lUnit)
    {
        UnitDetails unitDetails = lUnit.UnitDetails;
        UnitImage.sprite = unitDetails.Icon;
        _unitNameText.text = unitDetails.UnitName;
        UnitInformationPanel.gameObject.SetActive(true);
    }

    protected void OnTurnEnd(object sender, bool isNetworkInvoked) => DisableUnitInformationPanel();

    protected virtual void DisableUnitInformationPanel() => UnitInformationPanel.gameObject.SetActive(false);
}