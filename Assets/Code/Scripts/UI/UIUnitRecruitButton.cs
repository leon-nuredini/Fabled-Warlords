using System;
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

    [BoxGroup("Cost Text")] [SerializeField]
    private TextMeshProUGUI _costText;

    [BoxGroup("Cost Text")] [SerializeField]
    private Color _whiteColor = Color.white;

    [BoxGroup("Cost Text")] [SerializeField]
    private Color _redColor;

    private Button _button;

    #region Properties

    public LUnit LUnit      => _lUnit;
    public bool  CanRecruit { get; set; }

    #endregion

    private void Awake()
    {
        _button = GetComponent<Button>();
        _lUnit.InitProperties();
        UpdateCostText();
    }

    private void Start() => _button.onClick.AddListener(OnClickButton);
    

    public void UpdateButton(RecruitableUnits recruitableUnits)
    {
        switch (_lUnit)
        {
            case Archer archer:
                gameObject.SetActive(recruitableUnits.CanRecruitArcher);
                break;
            case Assassin assassin:
                gameObject.SetActive(recruitableUnits.CanRecruitAssassin);
                break;
            case AxeKnight axeKnight:
                gameObject.SetActive(recruitableUnits.CanRecruitAxeKnight);
                break;
            case Berserker berserker:
                gameObject.SetActive(recruitableUnits.CanRecruitBerserker);
                break;
            case LanceKnight lanceKnight:
                gameObject.SetActive(recruitableUnits.CanRecruitLanceKnight);
                break;
            case Spearman spearman:
                gameObject.SetActive(recruitableUnits.CanRecruitSpearman);
                break;
            case Swordsman swordsman:
                gameObject.SetActive(recruitableUnits.CanRecruitSwordsman);
                break;
            case Wizard wizard:
                gameObject.SetActive(recruitableUnits.CanRecruitWizard);
                break;
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
        _costText.text  = unitCost.ToString();
        _costText.color = CanRecruitUnit() ? _whiteColor : _redColor;
    }

    public bool CanRecruitUnit()
    {
        if (EconomyController.Instance == null) return false;
        if (!_button.interactable) return false;
        int unitCost     = _lUnit.UnitStats.Cost;
        int playerWealth = EconomyController.Instance.GetCurrentWealth(0);
        return playerWealth >= unitCost;
    }
}