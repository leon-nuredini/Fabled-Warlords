using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Players;
using UnityEngine.SceneManagement;

public class GameProgressSave : MonoBehaviour
{
    private int _currLevelIndex;

    private void OnEnable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.GameEnded += SaveProgress;
    }

    private void OnDisable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.GameEnded -= SaveProgress;
    }

    private void SaveProgress(object sender, GameEndedArgs e)
    {
        int playerNumber = e.gameResult.WinningPlayers[0];

        for (int i = 0; i < CellGrid.Instance.Players.Count; i++)
        {
            if (CellGrid.Instance.Players[i].PlayerNumber == playerNumber &&
                CellGrid.Instance.Players[i] is HumanPlayer)
            {
                int currentCompletedLevels = 1;
                if (PlayerPrefs.HasKey(SaveName.CompletedLevels))
                {
                    currentCompletedLevels = PlayerPrefs.GetInt(SaveName.CompletedLevels);
                    if (currentCompletedLevels < 1) currentCompletedLevels = 1;
                }
                
                int currentLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (currentCompletedLevels >= currentLevelIndex) return;
                PlayerPrefs.SetInt(SaveName.CompletedLevels, currentLevelIndex);

                break;
            }
        }
    }
}