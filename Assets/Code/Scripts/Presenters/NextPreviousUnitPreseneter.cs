using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

public class NextPreviousUnitPreseneter : MonoBehaviour
{
    public static Action OnAnyClickNextUnitButton;
    public static Action OnAnyClickPreviousUnitButton;

    [SerializeField] private GameObject _panel;
    private GraphicRaycaster _graphicRaycaster;

    [SerializeField] private Button _prevUnitButton;
    [SerializeField] private Button _nextUnitButton;

    private Tween _tween;

    private void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _prevUnitButton.onClick.AddListener(OnPreviousUnit);
        _nextUnitButton.onClick.AddListener(OnNextUnit);
    }

    private void OnEnable()
    {
        if (GameSettings.Instance != null)
            GameSettings.Instance.Preferences.OnUpdateUnitCyclingUI += UpdatePanel;
    }

    private void OnDisable()
    {
        if (GameSettings.Instance != null)
            GameSettings.Instance.Preferences.OnUpdateUnitCyclingUI -= UpdatePanel;
    }

    private void Start()
    {
        if (GameSettings.Instance != null)
            UpdatePanel(GameSettings.Instance.Preferences.EnableUnitCycleUI);
    }

    private void UpdatePanel(bool enable)
    {
        _panel.SetActive(enable);
        _graphicRaycaster.enabled = enable;
    } 

    private void OnPreviousUnit() => OnAnyClickPreviousUnitButton?.Invoke();
    private void OnNextUnit() => OnAnyClickNextUnitButton?.Invoke();
}