using System;
using TbsFramework.Grid;
using TMPro;
using UnityEngine;

public class UITurnText : MonoBehaviour
{
    [SerializeField] private TMP_Text _turnText;
    [SerializeField] private Color    _playerColor;
    [SerializeField] private Color    _aiColor;

    private ITween _tweener;

    private void Awake() => _tweener = GetComponentInChildren<ITween>(true);

    private void OnEnable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.TurnStarted += UpdateTurnText;
    }

    private void OnDisable()
    {
        if (CellGrid.Instance == null) return;
        CellGrid.Instance.TurnStarted -= UpdateTurnText;
    }

    private void UpdateTurnText(object sender, EventArgs eventArgs)
    {
        if (CellGrid.Instance == null) return;
        _turnText.text  = "AI Turn";
        _turnText.color = _aiColor;
        if (CellGrid.Instance.CurrentPlayerNumber == 0)
        {
            _turnText.text  = "Your Turn";
            _turnText.color = _playerColor;
        }
        
        _tweener?.Execute();
    }
}