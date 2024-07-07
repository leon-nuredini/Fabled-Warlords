using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TerrainDescriptionPresenter : MonoBehaviour
{
    public static event Action OnAnyOpenTerrainDescriptionPanel;
    public static event Action OnAnyCloseTerrainDescriptionPanel;

    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _closeButton;

    [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI _nameText;
    [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI _descriptionText;
    [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI _effectsText;
    [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI _movementCostText;
    [BoxGroup("Text")] [SerializeField] private TextMeshProUGUI _strategicTipText;
    [BoxGroup("Image")] [SerializeField] private Image _backgroundImage;
    [BoxGroup("Image")] [SerializeField] private Image _foregroundImage;

    private GraphicRaycaster _graphicRaycaster;
    [SerializeField] private bool _allowOpeningPanel = true;

    #region Properties

    public bool AllowOpeningPanel
    {
        get => _allowOpeningPanel;
        set => _allowOpeningPanel = value;
    }

    #endregion

    private void Awake()
    {
        _closeButton.onClick.AddListener(ClosePanel);
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        ClosePanel();
    }

    private void OnEnable()
    {
        UITutorial.OnAnyInspectTile += AllowTerrainInspection;

        if (ObjectHolder.Instance == null) return;
        ObjectHolder.Instance.OnSelectCell += OpenPanel;
    }

    private void OnDisable()
    {
        UITutorial.OnAnyInspectTile -= AllowTerrainInspection;

        if (ObjectHolder.Instance == null) return;
        ObjectHolder.Instance.OnSelectCell -= OpenPanel;
    }

    private void AllowTerrainInspection() => AllowOpeningPanel = true;

    private void OpenPanel(LSquare lSquare)
    {
        if (!AllowOpeningPanel) return;
        UpdateDetails(lSquare.TerrainDescription);
        _graphicRaycaster.enabled = true;
        _panel.SetActive(true);
        OnAnyOpenTerrainDescriptionPanel?.Invoke();
    }

    private void ClosePanel()
    {
        _graphicRaycaster.enabled = false;
        _panel.SetActive(false);
        OnAnyCloseTerrainDescriptionPanel?.Invoke();
    }

    private void UpdateDetails(TerrainDescription terrainDescription)
    {
        _nameText.text = terrainDescription.Name;
        _descriptionText.text = terrainDescription.Description;
        _effectsText.text = terrainDescription.Effect;
        _movementCostText.text = terrainDescription.MovementCost;
        _strategicTipText.text = terrainDescription.StrategicTip;
        _backgroundImage.sprite = terrainDescription.BackgroundSprite;
        _foregroundImage.sprite = terrainDescription.ForegroundSprite;

        _backgroundImage.enabled = _backgroundImage.sprite != null;
        _foregroundImage.enabled = _foregroundImage.sprite != null;
    }
}