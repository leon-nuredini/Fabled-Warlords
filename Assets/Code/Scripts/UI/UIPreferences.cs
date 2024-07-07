using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIPreferences : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button     _closeButton;

    [BoxGroup("Preferences Data")] [SerializeField]
    private Preferences _preferences;

    [BoxGroup("Setting Buttons")] [SerializeField]
    private Button _musicButton;

    [BoxGroup("Setting Buttons")] [SerializeField]
    private Button _sfxButton;

    [BoxGroup("Sliders")] [SerializeField] private Slider     _scrollSpeedSlider;
    [BoxGroup("Sliders")] [SerializeField] private Slider     _aiSpeedSlider;
    [BoxGroup("Toggles")] [SerializeField] private GameObject _musicToggle;
    [BoxGroup("Toggles")] [SerializeField] private GameObject _sfxToggle;

    private GraphicRaycaster _graphicRaycaster;
    private UIReturnToMenu   _uiReturnToMenu;

    protected GameObject     Panel          => _panel;
    protected UIReturnToMenu UIReturnToMenu => _uiReturnToMenu;

    protected virtual void Awake()
    {
        _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        _uiReturnToMenu   = GetComponent<UIReturnToMenu>();
        _scrollSpeedSlider.onValueChanged.AddListener(OnUpdateScrollSpeedSlider);
        _aiSpeedSlider.onValueChanged.AddListener(OnUpdateAISpeedSlider);
        _musicButton.onClick.AddListener(ToggleMusicVolume);
        _sfxButton.onClick.AddListener(ToggleSfxVolume);
        _closeButton.onClick.AddListener(ClosePreferencesPanel);
        ClosePreferencesPanel();
    }

    protected virtual void Start() => UpdatePreferences();

    private void UpdatePreferences()
    {
        _musicToggle.SetActive(_preferences.EnableMusic);
        _sfxToggle.SetActive(_preferences.EnableSfx);
        _scrollSpeedSlider.value = _preferences.ScrollSpeed;
        _aiSpeedSlider.value     = _preferences.AISpeed;
    }

    private void OnEnable()
    {
        UIMainMenu.OnClickPreferencesButton += OpenPreferencesPanel;
        UITop.OnAnyMenuButtonClicked        += OpenPreferencesPanel;
        if (_uiReturnToMenu == null) return;
        _uiReturnToMenu.OnAnyClickNoButton += OpenPreferencesPanel;
    }

    private void OnDisable()
    {
        UIMainMenu.OnClickPreferencesButton -= OpenPreferencesPanel;
        UITop.OnAnyMenuButtonClicked        -= OpenPreferencesPanel;
        if (_uiReturnToMenu == null) return;
        _uiReturnToMenu.OnAnyClickNoButton -= OpenPreferencesPanel;
    }

    private void OpenPreferencesPanel()
    {
        _graphicRaycaster.enabled = true;
        _panel.SetActive(true);
    }

    protected virtual void ClosePreferencesPanel()
    {
        _graphicRaycaster.enabled = false;
        _panel.SetActive(false);
    }

    private void OnUpdateScrollSpeedSlider(float value) => _preferences.ScrollSpeed = value;
    private void OnUpdateAISpeedSlider(float     value) => _preferences.AISpeed = value;

    private void ToggleMusicVolume()
    {
        _musicToggle.SetActive(!_musicToggle.activeSelf);
        _preferences.EnableMusic = _musicToggle.activeSelf;
    }

    private void ToggleSfxVolume()
    {
        _sfxToggle.SetActive(!_sfxToggle.activeSelf);
        _preferences.EnableSfx = _sfxToggle.activeSelf;
    }
}