using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIReturnToMenu : MonoBehaviour
{
    public event Action      OnAnyClickNoButton;
    public static event Action<int> OnAnyClickYesButton;

    [SerializeField]                       private GameObject _returnToMenuPanel;
    [BoxGroup("Buttons")] [SerializeField] private Button     _yesButton;
    [BoxGroup("Buttons")] [SerializeField] private Button     _noButton;

    [SerializeField] [Scene] private int _mainMenuIndex = 0;

    private void Awake()
    {
        _noButton.onClick.AddListener(ClosePanel);
        _yesButton.onClick.AddListener(ReturnToMainMenu);
    }
    
    public void OpenReturnToMenuPanel() => _returnToMenuPanel.SetActive(true);

    private void ClosePanel()
    {
        _returnToMenuPanel.SetActive(false);
        OnAnyClickNoButton?.Invoke();
    }

    private void ReturnToMainMenu() => OnAnyClickYesButton?.Invoke(_mainMenuIndex);
}