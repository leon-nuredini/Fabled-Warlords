using TbsFramework.Grid;
using TMPro;
using UnityEngine;

public class Level15ObjectivePresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _objectiveText;
    [SerializeField] private TextMeshProUGUI _attackText;

    [SerializeField] private string _objectiveDescription = "- Defeat all of the enemy units in the area";

    [SerializeField]
    private string _attackDescription = "- The Stormguard will attack in XX turn(s)";
    
    private void Awake()
    {
        _objectiveText.text = _objectiveDescription;
        _attackText.text = _attackDescription;
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
            _attackText.text = $"<s>{_attackText.text}</s>";
        }
    }

    private void UpdateInvasionDescription(int turnsUntilInvasion)
    {
        if (turnsUntilInvasion < 0) return;
        _attackText.text = _attackDescription;
        _attackText.text = _attackDescription.Replace("XX", $"{turnsUntilInvasion}");
        if (turnsUntilInvasion == 0)
            _attackText.text = $"<s>{_attackText.text}</s>";
    }
}