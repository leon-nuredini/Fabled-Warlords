using NaughtyAttributes;
using TMPro;
using UnityEngine;
using System;
using System.Collections;
using TbsFramework.Grid;
using TbsFramework.Players;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    #region Events

    public static event Action OnAnyMoveUnit;
    public static event Action OnAnyAllowEndingTurn;
    public static event Action OnAnyAttackEnemyUnit;
    public static event Action OnAnyCaptureVillage;
    public static event Action OnAnyDisplayVillageDescription;
    public static event Action OnAnyAllowRecruitment;
    public static event Action OnAnyToggleUnitDetails;
    public static event Action OnAnyInspectTile;

    #endregion

    private TutorialPart _tutorialPart = TutorialPart.Introduction;

    [BoxGroup("UI Elements")] [SerializeField]
    private GameObject _tutorialPanel;

    [BoxGroup("UI Elements")] [SerializeField]
    private TextMeshProUGUI _titleText;

    [BoxGroup("UI Elements")] [SerializeField]
    private TextMeshProUGUI _descriptionText;

    [BoxGroup("Tutorial Data")] [SerializeField]
    private TutorialData[] _tutorialDataArray;

    [BoxGroup("Tutorial Unit")] [SerializeField]
    private LUnit _spearmanUnit;

    [BoxGroup("Tutorial Unit")] [SerializeField]
    private LStructure _village;

    [BoxGroup("Objective")] [SerializeField]
    private GameObject _objectivePanel;

    [BoxGroup("Objective")] [SerializeField]
    private TextMeshProUGUI _objectiveDescriptionText;

    [BoxGroup("RecruitUnits")] [SerializeField]
    private Button _spearmanButton;

    [BoxGroup("RecruitUnits")] [SerializeField]
    private Button _archerButton;

    [BoxGroup("Upper Toolbar")] [SerializeField]
    private GameObject _upperToolbarImages;

    [BoxGroup("Unit Details")] [SerializeField]
    private GameObject _unitDetailsButtonImage;

    [BoxGroup("Terrain Details")] [SerializeField]
    private TerrainDescriptionPresenter _terrainDescriptionPresenter;

    private TutorialData _selectedTutorialData;
    private WaitForSeconds _wait;

    private bool _isInCoroutine;
    private bool _isTutorialFinished;
    private bool _lockTutorial;
    private bool _isArcherRecruited;
    private bool _isSpearmanRecruited;

    private void Awake()
    {
        _wait = new WaitForSeconds(.75f);

        _spearmanUnit.OnMove += TutorialSpearmanMoved;
        _spearmanUnit.OnAttack += TutorialSpearmanAttacked;
        _village.OnCaptured += TutorialVillageCaptured;

        _tutorialPanel.SetActive(false);
        _objectivePanel.SetActive(false);
        _upperToolbarImages.SetActive(false);
        _unitDetailsButtonImage.SetActive(false);
    }

    private void Start()
    {
        _terrainDescriptionPresenter.AllowOpeningPanel = false;
        _spearmanUnit.SetMovementPoints(0);
        UpdateTutorialPart();
    }

    private void OnEnable()
    {
        UITop.OnAnyEndTurnButtonClicked += TurnEnded;
        RecruitmentController.OnAnyPurchaseUnit += NewUnitPurchased;
        TerrainDescriptionPresenter.OnAnyOpenTerrainDescriptionPanel += DisableTutorialElements;
        TerrainDescriptionPresenter.OnAnyCloseTerrainDescriptionPanel += OnInspectTile;
    }

    private void OnDisable()
    {
        UITop.OnAnyEndTurnButtonClicked -= TurnEnded;
        RecruitmentController.OnAnyPurchaseUnit -= NewUnitPurchased;
        TerrainDescriptionPresenter.OnAnyOpenTerrainDescriptionPanel -= DisableTutorialElements;
        TerrainDescriptionPresenter.OnAnyCloseTerrainDescriptionPanel -= OnInspectTile;
    }

    private void Update()
    {
        if (_isTutorialFinished) return;
        if (_lockTutorial) return;
        if (_isInCoroutine) return;
        if (Input.anyKeyDown)
            AdvanceTutorial();
    }

    private void UpdateTutorialPart()
    {
        StartCoroutine(ShowTutorial());
    }

    private IEnumerator ShowTutorial()
    {
        _tutorialPanel.SetActive(false);
        _isInCoroutine = true;
        yield return _wait;
        switch (_tutorialPart)
        {
            case TutorialPart.Introduction:
                _selectedTutorialData = _tutorialDataArray[0];
                break;
            case TutorialPart.UnitStats:
                _selectedTutorialData = _tutorialDataArray[1];
                break;
            case TutorialPart.Goal:
                _selectedTutorialData = _tutorialDataArray[2];
                break;
            case TutorialPart.MoveUnit:
                _selectedTutorialData = _tutorialDataArray[3];
                _spearmanUnit.SetMovementPoints(_spearmanUnit.TotalMovementPoints);
                if (ObjectHolder.Instance.CurrSelectedUnit != null &&
                    ObjectHolder.Instance.CurrSelectedUnit.Equals(_spearmanUnit))
                    _spearmanUnit.HandleMouseDown();
                _objectivePanel.SetActive(true);
                _lockTutorial = true;
                OnAnyMoveUnit?.Invoke();
                break;
            case TutorialPart.Turns:
                _selectedTutorialData = _tutorialDataArray[4];
                OnAnyAllowEndingTurn?.Invoke();
                _objectivePanel.SetActive(true);
                break;
            case TutorialPart.Attacking:
                _selectedTutorialData = _tutorialDataArray[5];
                _objectivePanel.SetActive(true);
                OnAnyAttackEnemyUnit?.Invoke();
                break;
            case TutorialPart.CaptureVillage:
                _selectedTutorialData = _tutorialDataArray[6];
                _objectivePanel.SetActive(true);
                OnAnyCaptureVillage?.Invoke();
                break;
            case TutorialPart.Villages:
                _selectedTutorialData = _tutorialDataArray[7];
                _lockTutorial = false;
                _objectivePanel.SetActive(false);
                OnAnyDisplayVillageDescription?.Invoke();
                break;
            case TutorialPart.RecruitingUnits:
                _selectedTutorialData = _tutorialDataArray[8];
                OnAnyAllowRecruitment?.Invoke();
                _lockTutorial = true;
                _objectivePanel.SetActive(true);
                break;
            case TutorialPart.UnitAbilities:
                _selectedTutorialData = _tutorialDataArray[9];
                _lockTutorial = false;
                _objectivePanel.SetActive(false);
                break;
            case TutorialPart.TopUI:
                _selectedTutorialData = _tutorialDataArray[10];
                _lockTutorial = false;
                _upperToolbarImages.SetActive(true);
                break;
            case TutorialPart.TerainTypes:
                _selectedTutorialData = _tutorialDataArray[11];
                _lockTutorial = false;
                _upperToolbarImages.SetActive(false);
                break;
            case TutorialPart.InspectTerrain:
                _selectedTutorialData = _tutorialDataArray[12];
                _lockTutorial = true;
                _objectivePanel.SetActive(true);
                OnAnyInspectTile?.Invoke();
                break;
            case TutorialPart.ToggleUnitDetails:
                _selectedTutorialData = _tutorialDataArray[13];
                _unitDetailsButtonImage.SetActive(true);
                _lockTutorial = false;
                _objectivePanel.SetActive(false);
                OnAnyToggleUnitDetails?.Invoke();
                break;
            case TutorialPart.DefeatEnemy:
                _selectedTutorialData = _tutorialDataArray[14];
                _unitDetailsButtonImage.SetActive(false);
                _lockTutorial = false;
                break;
            default:
                _isTutorialFinished = true;
                _objectivePanel.SetActive(false);
                gameObject.SetActive(false);
                break;
        }

        _objectiveDescriptionText.text = _selectedTutorialData.ObjectiveDescription;
        UpdateTutorialData();
        _tutorialPanel.SetActive(true);
        _isInCoroutine = false;
    }

    private void DisableTutorialElements()
    {
        _objectivePanel.SetActive(false);
        _tutorialPanel.SetActive(false);
    }

    private void AdvanceTutorial()
    {
        _tutorialPart++;
        UpdateTutorialPart();
    }

    private void TutorialSpearmanMoved(UnitDirection direction)
    {
        if (_tutorialPart == TutorialPart.MoveUnit)
            AdvanceTutorial();
    }

    private void TurnEnded()
    {
        if (_tutorialPart == TutorialPart.Turns)
            AdvanceTutorial();
    }

    private void TutorialSpearmanAttacked(UnitDirection direction)
    {
        if (_tutorialPart == TutorialPart.Attacking)
            AdvanceTutorial();
    }

    private void TutorialVillageCaptured(LUnit agressor)
    {
        if (_tutorialPart == TutorialPart.Attacking)
            _tutorialPart = TutorialPart.CaptureVillage;
        if (_tutorialPart == TutorialPart.CaptureVillage)
            AdvanceTutorial();
    }

    private void NewUnitPurchased(LUnit newUnit, int playerNumber, int cost)
    {
        if (_tutorialPart != TutorialPart.RecruitingUnits) return;
        if (_isArcherRecruited && _isSpearmanRecruited) return;
        if (CellGrid.Instance == null) return;
        if (CellGrid.Instance.CurrentPlayer is not HumanPlayer) return;

        switch (newUnit)
        {
            case Archer archer:
                _isArcherRecruited = true;
                _archerButton.interactable = false;
                break;
            case Spearman spearman:
                _isSpearmanRecruited = true;
                _spearmanButton.interactable = false;
                break;
        }

        if (_isArcherRecruited && _isSpearmanRecruited)
        {
            _archerButton.interactable = true;
            _spearmanButton.interactable = true;
            _lockTutorial = false;
        }
    }

    private void OnInspectTile()
    {
        if (_tutorialPart == TutorialPart.InspectTerrain)
            AdvanceTutorial();
    }

    private void UpdateTutorialData()
    {
        _titleText.text = _selectedTutorialData.Title;
        _descriptionText.text = _selectedTutorialData.Description;
    }
}