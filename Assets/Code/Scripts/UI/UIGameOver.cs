using System;
using NaughtyAttributes;
using TbsFramework.Grid;
using TbsFramework.Players;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOver : MonoBehaviour
{
    public static event Action<int> OnAnyClickReturnToMenuButton;
    
    [SerializeField]         private TMP_Text _gameOverText;
    [SerializeField]         private TMP_Text _returnToMenuText;
    [SerializeField]         private Color    _victoryColor;
    [SerializeField]         private Color    _gameOverColor;
    [SerializeField]         private Button   _returnToMenuButton;
    
    [SerializeField] [Scene] private int      _mainMenuIndex = 0;

    private GraphicRaycaster _graphicRaycaster;
    private ITween[]         _tweenerArray;

    private readonly string _victory  = "Victory";
    private readonly string _gameOver = "Game Over";

    private void Awake()
    {
        _tweenerArray     = GetComponentsInChildren<ITween>(true);
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _returnToMenuButton.onClick.AddListener(ReturnToMenu);
    }

    private void OnEnable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.GameEnded += UpdateGameOverText;
    }

    private void OnDisable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.GameEnded -= UpdateGameOverText;
    }

    private void ReturnToMenu()
    {
        _graphicRaycaster.enabled = false;
        OnAnyClickReturnToMenuButton?.Invoke(_mainMenuIndex);
    }
    
    private void UpdateGameOverText(object sender, GameEndedArgs e)
    {
        if (CellGrid.Instance == null) return;
        _graphicRaycaster.enabled = true;

        int playerNumber = e.gameResult.WinningPlayers[0];

        _gameOverText.text      = _gameOver;
        _gameOverText.color     = _gameOverColor;
        _returnToMenuText.color = _gameOverColor;

        for (int i = 0; i < CellGrid.Instance.Players.Count; i++)
        {
            if (CellGrid.Instance.Players[i].PlayerNumber == playerNumber &&
                CellGrid.Instance.Players[i] is HumanPlayer)
            {
                _gameOverText.text      = _victory;
                _gameOverText.color     = _victoryColor;
                _returnToMenuText.color = _victoryColor;
                break;
            }
        }

        for (int i = 0; i < _tweenerArray.Length; i++)
            _tweenerArray[i].Execute();
    }
}