using NaughtyAttributes;
using TbsFramework.Grid;
using TMPro;
using UnityEngine;

public class Level10ObjectivePresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _firstObjectiveText;
    [SerializeField] private TextMeshProUGUI _secondObjectiveText;
    [SerializeField] private RectTransform _backgroundRect;
    [SerializeField] private float _fullHeight = 115.14f;

    [SerializeField] private string _firstObjectiveDescription = "- Reach the coastal castle";
    [SerializeField] private string _secondObjectiveDescription = "- Survive until turn XX";

    private LSurvivalCondition _survivalCondition;

    private void Awake()
    {
        _firstObjectiveText.text = _firstObjectiveDescription;
        _secondObjectiveText.text = "";

        _survivalCondition = FindObjectOfType<LSurvivalCondition>();
    }

    private void OnEnable()
    {
        ClaimBaseObjective.OnAnyCompleteClaimBaseObjective += OnFirstObjectiveCompleted;
        LSurvivalCondition.OnAnySurvivalConditionMet += OnSecondObjectiveCompleted;
    }

    private void OnDisable()
    {
        ClaimBaseObjective.OnAnyCompleteClaimBaseObjective -= OnFirstObjectiveCompleted;
        LSurvivalCondition.OnAnySurvivalConditionMet -= OnSecondObjectiveCompleted;
    }

    [Button("TEST")]
    private void OnFirstObjectiveCompleted()
    {
        _firstObjectiveText.text = $"<s>{_firstObjectiveText.text}</s>";
        int currentTurn = CellGrid.Instance.TurnNumber;
        int surviveUntilTurn = currentTurn + _survivalCondition.TurnToSurvive + 1;
        _secondObjectiveDescription = _secondObjectiveDescription.Replace("XX", $"{surviveUntilTurn}");
        _secondObjectiveText.text = _secondObjectiveDescription;
        _backgroundRect.sizeDelta = new Vector2(_backgroundRect.sizeDelta.x, _fullHeight);
    }

    private void OnSecondObjectiveCompleted()
    {
        _secondObjectiveText.text = $"<s>{_secondObjectiveDescription}</s>";
    }
}