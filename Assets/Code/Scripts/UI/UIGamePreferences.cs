using UnityEngine;
using UnityEngine.UI;

public class UIGamePreferences : UIPreferences
{
    [SerializeField] private Button _mainMenuButton;

    protected override void Awake()
    {
        base.Awake();
        _mainMenuButton.onClick.AddListener(OpenReturnToMainMenuPanel);
    }

    private void OpenReturnToMainMenuPanel()
    {
        ClosePreferencesPanel();
        UIReturnToMenu.OpenReturnToMenuPanel();
    }

    protected override void ClosePreferencesPanel() => Panel.SetActive(false);
}