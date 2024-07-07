using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class ToggleUnitDetailsPresenter : MonoBehaviour
{
    public static event Action<bool> OnAnyToggleUnitDetails;

    [BoxGroup] [SerializeField] private Image  _backgroundImage;
    [BoxGroup] [SerializeField] private Image  _iconImage;
    [BoxGroup] [SerializeField] private Button _button;

    [BoxGroup("Sprite")] [SerializeField] private Sprite _checkmarkSprite;
    [BoxGroup("Sprite")] [SerializeField] private Sprite _crossSprite;

    [BoxGroup("Colors")] [SerializeField] private Color _enabledColor;
    [BoxGroup("Colors")] [SerializeField] private Color _disabledColor;

    private bool _enableUnitInformation;

    private bool _allowTogglingUnitDetails = true;

    #region Properties

    public bool AllowTogglingUnitDetails { get => _allowTogglingUnitDetails; set => _allowTogglingUnitDetails = value; }

    #endregion

    public bool EnableUnitInformation
    {
        get => _enableUnitInformation;
        set
        {
            _enableUnitInformation = value;
            OnAnyToggleUnitDetails?.Invoke(_enableUnitInformation);
        }
    }

    private void Awake()
    {
        LoadData();
        _backgroundImage.color = _enabledColor;
        _button.onClick.AddListener(ToggleUnitInformation);
        UpdateButtonColor();
        UpdateIconSprite();
    }

    private void Update()
    {
        if (!AllowTogglingUnitDetails) return;
        if (Input.GetKeyDown(KeyCode.I))
            ToggleUnitInformation();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey(SaveName.ToggleUnitDetails))
            EnableUnitInformation = PlayerPrefs.GetInt(SaveName.ToggleUnitDetails) == 1;
        else
            EnableUnitInformation = true;
    }

    private void SaveData() => PlayerPrefs.Save();

    private void ToggleUnitInformation()
    {
        EnableUnitInformation = !EnableUnitInformation;
        PlayerPrefs.SetInt(SaveName.ToggleUnitDetails, EnableUnitInformation ? 1 : 0);
        UpdateButtonColor();
        UpdateIconSprite();
        SaveData();
    }

    private void UpdateButtonColor() =>
        _backgroundImage.color = EnableUnitInformation ? _enabledColor : _disabledColor;

    private void UpdateIconSprite() =>
        _iconImage.sprite = EnableUnitInformation ? _checkmarkSprite : _crossSprite;
}