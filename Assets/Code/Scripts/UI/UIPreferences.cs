using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPreferences : MonoBehaviour
{
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _closeButton;

    [BoxGroup("Preferences Data")] [SerializeField]
    private Preferences _preferences;

    [BoxGroup("Setting Buttons")] [SerializeField]
    private Button _musicButton;

    [BoxGroup("Setting Buttons")] [SerializeField]
    private Button _sfxButton;

    [BoxGroup("Setting Buttons")] [SerializeField]
    private Button _unitGlowButton;

    [BoxGroup("Setting Buttons")] [SerializeField]
    private Button _unitCycleUIButton;

    [BoxGroup("Setting Buttons")] [SerializeField]
    private Button _cameraTrackingButton;

    [BoxGroup("Sliders")] [SerializeField] private Slider _scrollSpeedSlider;
    [BoxGroup("Sliders")] [SerializeField] private Slider _aiSpeedSlider;
    [BoxGroup("Sliders")] [SerializeField] private TextMeshProUGUI _aiSpeedValueText;
    [BoxGroup("Toggles")] [SerializeField] private GameObject _musicToggle;
    [BoxGroup("Toggles")] [SerializeField] private GameObject _sfxToggle;
    [BoxGroup("Toggles")] [SerializeField] private GameObject _unitGlowToggle;
    [BoxGroup("Toggles")] [SerializeField] private GameObject _unitCycleToggle;
    [BoxGroup("Toggles")] [SerializeField] private GameObject _cameraTrackingToggle;

    private GraphicRaycaster _graphicRaycaster;
    private UIReturnToMenu _uiReturnToMenu;

    protected GameObject Panel => _panel;
    protected UIReturnToMenu UIReturnToMenu => _uiReturnToMenu;

    #region Properties

    #endregion

    protected virtual void Awake()
    {
        _graphicRaycaster = GetComponentInParent<GraphicRaycaster>();
        _uiReturnToMenu = GetComponent<UIReturnToMenu>();
        _scrollSpeedSlider.onValueChanged.AddListener(OnUpdateScrollSpeedSlider);
        _aiSpeedSlider.onValueChanged.AddListener(OnUpdateAISpeedSlider);
        _musicButton.onClick.AddListener(ToggleMusicVolume);
        _sfxButton.onClick.AddListener(ToggleSfxVolume);
        _unitGlowButton.onClick.AddListener(ToggleUnitGlow);
        _unitCycleUIButton.onClick.AddListener(ToggleUnitCycleUI);
        _cameraTrackingButton.onClick.AddListener(ToggleCameraTracking);
        _closeButton.onClick.AddListener(ClosePreferencesPanel);
        ClosePreferencesPanel();
    }

    protected virtual void Start() => UpdatePreferences();

    private void UpdatePreferences()
    {
        _musicToggle.SetActive(_preferences.EnableMusic);
        _sfxToggle.SetActive(_preferences.EnableSfx);
        _unitGlowToggle.SetActive(_preferences.EnableUnitGlow);
        _unitCycleToggle.SetActive(_preferences.EnableUnitCycleUI);
        _cameraTrackingToggle.SetActive(_preferences.EnableCameraTracking);
        _scrollSpeedSlider.value = _preferences.ScrollSpeed;
        _aiSpeedSlider.value = _preferences.AISpeed;
        UpdateAISpeedValueText(_preferences.AISpeed);
    }

    private void OnEnable()
    {
        UIMainMenu.OnClickPreferencesButton += OpenPreferencesPanel;
        UITop.OnAnyMenuButtonClicked += OpenPreferencesPanel;
        if (_uiReturnToMenu == null) return;
        _uiReturnToMenu.OnAnyClickNoButton += OpenPreferencesPanel;
    }

    private void OnDisable()
    {
        UIMainMenu.OnClickPreferencesButton -= OpenPreferencesPanel;
        UITop.OnAnyMenuButtonClicked -= OpenPreferencesPanel;
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

    private void OnUpdateAISpeedSlider(float value)
    {
        _preferences.AISpeed = value;
        UpdateAISpeedValueText(value);
    }

    private void UpdateAISpeedValueText(float value) => _aiSpeedValueText.text = $"x{value:F1}";

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

    private void ToggleUnitGlow()
    {
        _unitGlowToggle.SetActive(!_unitGlowToggle.activeSelf);
        _preferences.EnableUnitGlow = _unitGlowToggle.activeSelf;
    }

    private void ToggleUnitCycleUI()
    {
        _unitCycleToggle.SetActive(!_unitCycleToggle.activeSelf);
        _preferences.EnableUnitCycleUI = _unitCycleToggle.activeSelf;
    }

    private void ToggleCameraTracking()
    {
        _cameraTrackingToggle.SetActive(!_cameraTrackingToggle.activeSelf);
        _preferences.EnableCameraTracking = _cameraTrackingToggle.activeSelf;
    }
}