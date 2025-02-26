using System;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIUnitRecruitButton : MonoBehaviour
{
    public event Action<UIUnitRecruitButton> OnButtonSelected;

    public static event Action<LUnit> OnAnySelectUnit;

    [BoxGroup("Unit")] [SerializeField] private LUnit _lUnit;

    [BoxGroup("Button Graphics")] [SerializeField]
    private GameObject[] _displayGraphics;

    [BoxGroup("Name Text")] [SerializeField]
    private TextMeshProUGUI _nameText;

    [BoxGroup("Cost Text")] [SerializeField]
    private TextMeshProUGUI _costText;

    [BoxGroup("Image")] [SerializeField] private Image _unitImage;

    [BoxGroup("Cost Text")] [SerializeField]
    private Color _whiteColor = Color.white;

    [BoxGroup("Cost Text")] [SerializeField]
    private Color _redColor;

    private Button _button;

    #region Properties

    public LUnit LUnit => _lUnit;
    public bool CanRecruit { get; set; }

    #endregion

    private void Awake()
    {
        _button = GetComponent<Button>();
        _lUnit.InitProperties();
        UpdateCostText();
    }

    private void Start() => _button.onClick.AddListener(OnClickButton);

    public void UpdateButton(List<RecruitableUnit> recruitableUnits)
    {
        gameObject.SetActive(false);
        for (int i = 0; i < recruitableUnits.Count; i++)
        {
            if (recruitableUnits[i].LUnit.Equals(_lUnit) && recruitableUnits[i].CanRecruit)
                gameObject.SetActive(true);
        }
    }

    private void OnClickButton() => SelectButton();

    public void SelectButton()
    {
        OnButtonSelected?.Invoke(this);
        for (int i = 0; i < _displayGraphics.Length; i++)
            _displayGraphics[i].SetActive(true);

        OnAnySelectUnit?.Invoke(CanRecruitUnit() ? _lUnit : null);
    }

    public void DeselectButton()
    {
        for (int i = 0; i < _displayGraphics.Length; i++)
            _displayGraphics[i].SetActive(false);
    }

    public void UpdateCostText()
    {
        int unitCost = _lUnit.UnitStats.Cost;
        _costText.text = unitCost.ToString();
        _costText.color = CanRecruitUnit() ? _whiteColor : _redColor;
        _nameText.text = _lUnit.UnitDetails.UnitName;
        _unitImage.sprite = _lUnit.UnitDetails.OverworldIcon;
    }

    public bool CanRecruitUnit()
    {
        if (EconomyController.Instance == null) return false;
        if (!_button.interactable) return false;
        int unitCost = _lUnit.UnitStats.Cost;
        int playerWealth = EconomyController.Instance.GetCurrentWealth(0);
        return playerWealth >= unitCost;
    }
}