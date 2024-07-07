using System;
using System.Collections;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UITop : MonoBehaviour
{
    public static event Action OnAnyRecruitButtonClicked;
    public static event Action OnAnyMenuButtonClicked;
    public static event Action OnAnyEndTurnButtonClicked;

    [SerializeField] private TextMeshProUGUI _clockText;

    [BoxGroup("Buttons")] [SerializeField] private Button _menuButton;
    [BoxGroup("Buttons")] [SerializeField] private Button _recruitButton;
    [BoxGroup("Buttons")] [SerializeField] private Button _endTurnButton;

    private WaitForSeconds _waitSixtySeconds;
    private CanvasGroup    _endTurnCanvasGroup;

    private int secondsPassed;
    private int secondsToWait = 60;

    private bool _allowEndTurn = true;
    private bool _allowTogglingUnitDetails = true;

    #region Properties

    public bool AllowEndTurn { get => _allowEndTurn; set => _allowEndTurn = value; }
    
    #endregion

    private void Awake()
    {
        _waitSixtySeconds   = new WaitForSeconds(secondsToWait);
        _endTurnCanvasGroup = _endTurnButton.GetComponent<CanvasGroup>();
        _menuButton.onClick.AddListener(OpenSettingsPanel);
        _recruitButton.onClick.AddListener(OpenRecruitmentPanel);
        _endTurnButton.onClick.AddListener(EndTurn);
    }

    private void OnEnable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.TurnStarted += EnableEndTurnButton;
    }

    private void OnDisable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.TurnStarted -= EnableEndTurnButton;
    }

    private void Start()
    {
        _clockText.text = "00:00";
        StartCoroutine(UpdateTimePassed());
    }

    void Update()
    {
        if (!AllowEndTurn) return;
        if (Input.GetKeyDown(KeyCode.M)) { EndTurn(); }
    }

    private IEnumerator UpdateTimePassed()
    {
        while (true)
        {
            yield return _waitSixtySeconds;
            secondsPassed += secondsToWait;
            TimeSpan time = TimeSpan.FromSeconds(secondsPassed);
            _clockText.text = $"{time.Hours:D2}:{time.Minutes:D2}";
        }
    }

    private void OpenSettingsPanel()    => OnAnyMenuButtonClicked?.Invoke();
    private void OpenRecruitmentPanel() => OnAnyRecruitButtonClicked?.Invoke();

    private void EndTurn()
    {
        if (CellGrid.Instance == null) return;
        if (CellGrid.Instance.cellGridState is CellGridStateAITurn) return;
        OnAnyEndTurnButtonClicked?.Invoke();
        CellGrid.Instance.EndTurn();
        DisableEndTurnButton();
    }

    private void EnableEndTurnButton(object sender, EventArgs eventArgs)
    {
        if (CellGrid.Instance.CurrentPlayer is AIPlayer) return;
        _endTurnCanvasGroup.alpha   = 1f;
        _endTurnButton.interactable = true;
    }

    private void DisableEndTurnButton()
    {
        _endTurnCanvasGroup.alpha   = .5f;
        _endTurnButton.interactable = false;
    }
}