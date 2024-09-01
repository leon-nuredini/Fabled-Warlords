using TbsFramework.Grid;
using TMPro;
using UnityEngine;

public class Level3ObjectivePresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _objectiveText;

    [SerializeField] private string _objectiveDescription = "- Defeat the invading beastmen";

    private void Awake()
    {
        _objectiveText.text = _objectiveDescription;
    }

    private void OnEnable()
    {
        CellGrid.Instance.GameEnded += OnObjectiveCompleted;
    }

    private void OnDisable()
    {
        CellGrid.Instance.GameEnded -= OnObjectiveCompleted;
    }
    
    private void OnObjectiveCompleted(object obj, GameEndedArgs gameEndedArgs)
    {
        //if the winner is the player
        if (gameEndedArgs.gameResult.WinningPlayers.Contains(0))
            _objectiveText.text = $"<s>{_objectiveText.text}</s>";
    }
}