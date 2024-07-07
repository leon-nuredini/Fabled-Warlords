using UnityEngine;
using UnityEngine.UI;
using System;

public class UIMainMenu : MonoBehaviour
{
    public static event Action OnClickCampaignButton;
    public static event Action OnClickPreferencesButton;
    public static event Action OnClickCreditsButton;

    [SerializeField] private Button _campaignButton;
    [SerializeField] private Button _preferencesButton;
    [SerializeField] private Button _creditsButton;

    private void Awake()
    {
        _campaignButton.onClick.AddListener(OpenCampaignPanel);
        _preferencesButton.onClick.AddListener(OpenPreferencesPanel);
        _creditsButton.onClick.AddListener(OpenCreditsPanel);
    }

    private void OpenCampaignPanel()    => OnClickCampaignButton?.Invoke();
    private void OpenPreferencesPanel() => OnClickPreferencesButton?.Invoke();
    private void OpenCreditsPanel()     => OnClickCreditsButton?.Invoke();
}