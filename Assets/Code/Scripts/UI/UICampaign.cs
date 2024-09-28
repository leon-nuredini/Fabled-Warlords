using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;
using NaughtyAttributes;

public class UICampaign : MonoBehaviour
{
    public static event Action<int> OnAnyClickedPlayButton;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _cancelButton;
    [SerializeField] private TextMeshProUGUI _levelDescriptionText;
    [SerializeField] private ScrollRect _scrollRect;

    [BoxGroup("Unlock all levels")] [SerializeField]
    private bool _unlockAllLevels;

    private UILevelButton[] _uiLevelButtonArray;

    private Scrollbar _scrollBar;
    private GraphicRaycaster _graphicRaycaster;

    private int _selectedLevelIndex;
    private int _unlockedLevelsAmount = 1;
    private bool _isOpenedFirstTime;

    private void Awake() => _scrollBar = _scrollRect.gameObject.GetComponentInChildren<Scrollbar>();

    private void Start()
    {
        _uiLevelButtonArray = GetComponentsInChildren<UILevelButton>(true);
        _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        _cancelButton.onClick.AddListener(CloseCampaignPanel);
        _playButton.onClick.AddListener(PlayLevel);
        CloseCampaignPanel();
        if (_unlockAllLevels)
            _unlockedLevelsAmount = 999;
        else
            LoadData();

        for (int i = 0; i < _uiLevelButtonArray.Length; i++)
        {
            if (_uiLevelButtonArray[i].LevelDetails.LevelIndex - 1 > _unlockedLevelsAmount - 1)
                _uiLevelButtonArray[i].Lock();

            _uiLevelButtonArray[i].DeselectLevelButton();
            _uiLevelButtonArray[i].OnLevelSelected += OnSelectLevel;
        }

        _scrollBar.value = 1f;
        if (LevelManager.openLevelSelection)
        {
            LevelManager.openLevelSelection = false;
            OpenCampaignPanel();
            for (int i = 0; i < _uiLevelButtonArray.Length; i++)
            {
                if (_uiLevelButtonArray[i].Button.interactable)
                {
                    _uiLevelButtonArray[i].SelectLevelButton();
                    _scrollRect.FocusOnItem(_uiLevelButtonArray[i].RectTransform);
                }
            }
        }
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(SaveName.CompletedLevels))
            _unlockedLevelsAmount = Math.Clamp(PlayerPrefs.GetInt(SaveName.CompletedLevels), 2, 999);
    }

    private void OnEnable() => UIMainMenu.OnClickCampaignButton += OpenCampaignPanel;

    private void OnDisable() => UIMainMenu.OnClickCampaignButton -= OpenCampaignPanel;

    private void OnSelectLevel(UILevelButton levelButton)
    {
        for (int i = 0; i < _uiLevelButtonArray.Length; i++)
        {
            if (levelButton.Equals(_uiLevelButtonArray[i])) continue;
            _uiLevelButtonArray[i].DeselectLevelButton();
        }

        _levelDescriptionText.text = levelButton.LevelDetails.LevelDescription;
        _selectedLevelIndex = levelButton.LevelDetails.LevelIndex;
    }

    private void OpenCampaignPanel()
    {
        _graphicRaycaster.enabled = true;
        _panel.SetActive(true);
        _uiLevelButtonArray[0].SelectLevelButton();
        if (!_isOpenedFirstTime)
        {
            _isOpenedFirstTime = true;
            return;
        }

        _scrollRect.FocusOnItem(_uiLevelButtonArray[0].RectTransform);
    }

    private void CloseCampaignPanel()
    {
        _graphicRaycaster.enabled = false;
        _panel.SetActive(false);
    }

    private void PlayLevel() => OnAnyClickedPlayButton?.Invoke(_selectedLevelIndex);
}