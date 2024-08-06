using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Lean.Pool;
using NaughtyAttributes;
using TbsFramework.Grid;
using UnityEngine.UI;

public class UIRecruitment : MonoBehaviour
{
    public event Action<LUnit> OnUpdateUnitDetails;

    public static event Action OnAnyOpenRecruitmentPanel;
    public static event Action OnAnyClickRecruitButton;

    [BoxGroup("Buttons")] [SerializeField] private Button _recruitButton;
    [BoxGroup("Buttons")] [SerializeField] private Button _closeButton;

    [SerializeField] private GameObject _recruitmentPanel;
    [SerializeField] private GameObject _recruitButtonsPanel;
    [SerializeField] private UIUnitRecruitButton _selectedUnitRecruitButton;

    private List<UIUnitRecruitButton> _unitRecruitButtonArray = new List<UIUnitRecruitButton>();

    private bool _allowRecruitment = true;

    private RecruitmentController _recruitmentController;

    #region Properties

    public bool AllowRecruitment
    {
        get => _allowRecruitment;
        set => _allowRecruitment = value;
    }

    #endregion

    private void Awake()
    {
        SpawnUnitButtons();
        _closeButton.onClick.AddListener(ClosePanel);
        _recruitButton.onClick.AddListener(ClosePanel);
        _recruitButton.onClick.AddListener(OnClickRecruitButton);
        ClosePanel();
    }

    private void SpawnUnitButtons()
    {
        if (Factions.Instance == null)
        {
            Debug.LogWarning("Factions instance is null. Ensure the Factions gameObject exists in the scene.");
            return;
        }

        PlayerFaction playerFaction = Factions.Instance.PlayerFaction;

        if (playerFaction == null)
        {
            Debug.LogWarning("PlayerFaction is null! Assign a PlayerFaction in the Factions gameObject property.");
            return;
        }

        GameObject recruitButtons =
            LeanPool.Spawn(playerFaction.FactionRecruitmentPanel, _recruitButtonsPanel.transform);

        _selectedUnitRecruitButton = recruitButtons.GetComponentInChildren<UIUnitRecruitButton>();
        _unitRecruitButtonArray = GetComponentsInChildren<UIUnitRecruitButton>().ToList();
        if (_recruitmentController == null)
            _recruitmentController = FindObjectOfType<RecruitmentController>();
        UpdateButtons();
    }

    private void OnEnable()
    {
        //RecruitmentController.OnAnyUpdateRecruitableUnits += UpdateButtons;
        UITop.OnAnyRecruitButtonClicked += OpenRecruitmentPanel;
        UITop.OnAnyMenuButtonClicked += ClosePanel;

        for (int i = 0; i < _unitRecruitButtonArray.Count; i++)
            _unitRecruitButtonArray[i].OnButtonSelected += OnSelectButton;

        if (CellGrid.Instance != null) CellGrid.Instance.TurnEnded += OnTurnEnded;
    }

    private void OnDisable()
    {
        //RecruitmentController.OnAnyUpdateRecruitableUnits -= UpdateButtons;
        UITop.OnAnyRecruitButtonClicked -= OpenRecruitmentPanel;
        UITop.OnAnyMenuButtonClicked -= ClosePanel;

        for (int i = 0; i < _unitRecruitButtonArray.Count; i++)
            _unitRecruitButtonArray[i].OnButtonSelected -= OnSelectButton;

        if (CellGrid.Instance != null) CellGrid.Instance.TurnEnded -= OnTurnEnded;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            OpenRecruitmentPanel();
    }

    private void UpdateButtons()
    {
        switch (Factions.Instance.PlayerFaction.FactionType)
        {
            case FactionType.Human:
                for (int i = 0; i < _unitRecruitButtonArray.Count; i++)
                    _unitRecruitButtonArray[i]
                        .UpdateButton(_recruitmentController.HumanUnits.RecruitableUnitList);
                break;
            case FactionType.Beastmen:
                for (int i = 0; i < _unitRecruitButtonArray.Count; i++)
                    _unitRecruitButtonArray[i]
                        .UpdateButton(_recruitmentController.BeastmenUnits.RecruitableUnitList);
                break;
            case FactionType.Primordial:
                for (int i = 0; i < _unitRecruitButtonArray.Count; i++)
                    _unitRecruitButtonArray[i]
                        .UpdateButton(_recruitmentController.PrimordialUnits.RecruitableUnitList);
                break;
        }
    }

    private void OnTurnEnded(object sender, bool isNetworkInvoked) => ClosePanel();

    private void OpenRecruitmentPanel()
    {
        if (!AllowRecruitment) return;
        if (CellGrid.Instance != null && CellGrid.Instance.CurrentPlayerNumber != 0) return;
        if (_recruitmentPanel.activeSelf)
        {
            ClosePanel();
            return;
        }

        if (_selectedUnitRecruitButton == null) return;
        _selectedUnitRecruitButton.SelectButton();

        for (int i = 0; i < _unitRecruitButtonArray.Count; i++)
            _unitRecruitButtonArray[i].UpdateCostText();

        OnAnyOpenRecruitmentPanel?.Invoke();
    }

    private void OnSelectButton(UIUnitRecruitButton selectedButton)
    {
        for (int i = 0; i < _unitRecruitButtonArray.Count; i++)
        {
            if (_unitRecruitButtonArray[i] == selectedButton) continue;
            _unitRecruitButtonArray[i].DeselectButton();
        }

        _recruitButton.interactable = selectedButton.CanRecruitUnit();
        if (_recruitButton.TryGetComponent(out CanvasGroup canvasGroup))
            canvasGroup.alpha = _recruitButton.interactable ? 1f : 0.5f;

        OnUpdateUnitDetails?.Invoke(selectedButton.LUnit);
    }

    private void OnClickRecruitButton() => OnAnyClickRecruitButton?.Invoke();

    private void ClosePanel() => _recruitmentPanel.gameObject.SetActive(false);
}