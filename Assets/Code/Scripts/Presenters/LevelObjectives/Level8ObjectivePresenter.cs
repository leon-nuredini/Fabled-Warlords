using TbsFramework.Grid;
using TMPro;
using UnityEngine;

public class Level8ObjectivePresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _objectiveText;
    [SerializeField] private TextMeshProUGUI _primordialInvasionText;

    [SerializeField] private string _objectiveDescription = "- Defeat all of the enemy units in the area";

    [SerializeField]
    private string _primordialInvasionDescription = "- The primordials will start their invasion in XX turn(s)";
    
    private void Awake()
    {
        _objectiveText.text = _objectiveDescription;
        _primordialInvasionText.text = _primordialInvasionDescription;
    }

    private void OnEnable()
    {
        CellGrid.Instance.GameEnded += OnObjectiveCompleted;
        UnitInvasionStarter.OnAnyTurnUntilInvasionUpdated += UpdateInvasionDescription;
    }

    private void OnDisable()
    {
        CellGrid.Instance.GameEnded -= OnObjectiveCompleted;
        UnitInvasionStarter.OnAnyTurnUntilInvasionUpdated -= UpdateInvasionDescription;
    }

    private void OnObjectiveCompleted(object obj, GameEndedArgs gameEndedArgs)
    {
        //if the winner is the player
        if (gameEndedArgs.gameResult.WinningPlayers.Contains(0))
        {
            _objectiveText.text = $"<s>{_objectiveText.text}</s>";
            _primordialInvasionText.text = $"<s>{_primordialInvasionText.text}</s>";
        }
    }

    private void UpdateInvasionDescription(int turnsUntilInvasion)
    {
        if (turnsUntilInvasion < 0) return;
        _primordialInvasionText.text = _primordialInvasionDescription;
        _primordialInvasionText.text = _primordialInvasionDescription.Replace("XX", $"{turnsUntilInvasion}");
        if (turnsUntilInvasion == 0)
            _primordialInvasionText.text = $"<s>{_primordialInvasionText.text}</s>";
    }
}