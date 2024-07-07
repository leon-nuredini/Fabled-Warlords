using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using NaughtyAttributes;

public class UICampaign : MonoBehaviour
{
    public static event Action<int> OnAnyClickedPlayButton;

    [SerializeField] private GameObject      _panel;
    [SerializeField] private Button          _playButton;
    [SerializeField] private Button          _cancelButton;
    [SerializeField] private TextMeshProUGUI _levelDescriptionText;

    [BoxGroup("Unlock all levels")] [SerializeField]
    private bool _unlockAllLevels;
    
    private UILevelButton[] _uiLevelButtonArray;

    private GraphicRaycaster _graphicRaycaster;

    private int _selectedLevelIndex;
    private int _unlockedLevelsAmount = 1;

    private void Start()
    {
        _uiLevelButtonArray = GetComponentsInChildren<UILevelButton>(true);
        _graphicRaycaster   = GetComponentInParent<GraphicRaycaster>();
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

        if (LevelManager.openLevelSelection)
        {
            LevelManager.openLevelSelection = false;
            OpenCampaignPanel();
            for (int i = 0; i < _uiLevelButtonArray.Length; i++)
            {
                if (_uiLevelButtonArray[i].Button.interactable)
                    _uiLevelButtonArray[i].SelectLevelButton();
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
        _selectedLevelIndex        = levelButton.LevelDetails.LevelIndex;
    }

    private void OpenCampaignPanel()
    {
        _graphicRaycaster.enabled = true;
        _panel.SetActive(true);
        _uiLevelButtonArray[0].SelectLevelButton();
    }

    private void CloseCampaignPanel()
    {
        _graphicRaycaster.enabled = false;
        _panel.SetActive(false);
    }

    private void PlayLevel() => OnAnyClickedPlayButton?.Invoke(_selectedLevelIndex);
}