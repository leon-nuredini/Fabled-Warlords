using NaughtyAttributes;
using TbsFramework.Grid;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitDataPresenter : BaseUnitDataPresenter
{
    [SerializeField] private TextMeshProUGUI _unitDescriptionText;

    [BoxGroup("Image Size")] [SerializeField]
    private Vector2 _unitImageSize = new Vector2(225f, 225f);

    [BoxGroup("Unit Type Image")] [SerializeField]
    private Image _unitTypeImage;

    [BoxGroup("Image Size")] [SerializeField]
    private Vector2 _structureImageSize = new Vector2(125f, 125f);

    [BoxGroup("Pin Button")] [SerializeField]
    private Button _pinButton;

    private bool _pinStats;
    private bool _showUnitDetailsPanel;

    #region Properties

    public bool PinStats
    {
        get => _pinStats;
        set
        {
            _pinStats = value;
            if (!_pinStats) DisableUnitInformationPanel();
        }
    }

    #endregion

    private void Awake()
    {
        DisableUnitInformationPanel();
        _pinButton.onClick.AddListener(OnClickPinStatsButton);
    }

    private void OnEnable()
    {
        LUnit.OnAnyDisplayUnitInformation += UpdateUnitData;
        LUnit.OnAnyHideUnitInformation += DisableUnitInformationPanel;
        ToggleUnitDetailsPresenter.OnAnyToggleUnitDetails += OnToggleUnitInformationPanel;
        CellGrid.Instance.TurnEnded += OnTurnEnd;
    }

    private void OnDisable()
    {
        LUnit.OnAnyDisplayUnitInformation -= UpdateUnitData;
        LUnit.OnAnyHideUnitInformation -= DisableUnitInformationPanel;
        ToggleUnitDetailsPresenter.OnAnyToggleUnitDetails -= OnToggleUnitInformationPanel;
        CellGrid.Instance.TurnEnded -= OnTurnEnd;
    }

    protected override void UpdateUnitData(LUnit lUnit)
    {
        if (!_showUnitDetailsPanel) return;

        if (lUnit is LStructure)
            UnitImage.rectTransform.sizeDelta = _structureImageSize;
        else
            UnitImage.rectTransform.sizeDelta = _unitImageSize;

        _unitDescriptionText.text = lUnit.UnitDetails.Description;

        Sprite unitTypeSprite = lUnit.UnitClassCounter.UnitTypeSprite;
        if (unitTypeSprite != null) 
            _unitTypeImage.sprite = lUnit.UnitClassCounter.UnitTypeSprite;
        else
            _unitTypeImage.enabled = false;
        
        base.UpdateUnitData(lUnit);
    }

    private void OnClickPinStatsButton() => PinStats = !PinStats;

    protected override void DisableUnitInformationPanel()
    {
        if (_pinStats) return;
        base.DisableUnitInformationPanel();
    }

    private void OnToggleUnitInformationPanel(bool showUnitDetailsPanel)
    {
        if (!showUnitDetailsPanel) DisableUnitInformationPanel();
        _showUnitDetailsPanel = showUnitDetailsPanel;
    }
}