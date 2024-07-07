using System.Collections.Generic;
using Singleton;
using TbsFramework.Grid;
using TbsFramework.Players;
using UnityEngine;
using System;

public class EconomyController : SceneSingleton<EconomyController>
{
    public static event Action<PlayerAccount> OnAnyUpdateWealth;
    public static event Action<int>           OnAnyUpkeepUpdated;
    public static event Action<int>           OnAnyNetIncomeUpdated;

    private Dictionary<int, PlayerAccount> _accountDictionary = new Dictionary<int, PlayerAccount>();
    private CellGrid                       _cellGrid;

    [SerializeField] private int _humanStartingAmount = 0;
    [SerializeField] private int _aiStartingAmount    = 0;
    [SerializeField] private int _defaultPlayerAmount = 0;
    [SerializeField] private int _defaultAIAmount     = 0;

    public Dictionary<int, PlayerAccount> AccountDictionary => _accountDictionary;

    private void Awake()
    {
        if (CellGrid.Instance == null) return;
        _cellGrid                               =  CellGrid.Instance;
        _cellGrid.GameStarted                   += OnGameStarted;
        _cellGrid.TurnStarted                   += OnTurnStart;
        _cellGrid.OnUnitAdded                   += UpdateUpkeepAndNetIncome;
        _cellGrid.OnUnitRemoved                 += UpdateUpkeepAndNetIncome;
        LStructure.OnAnyCapturedStructure       += UpdatePlayerNetIncome;
        RecruitmentController.OnAnyPurchaseUnit += UpdateCurrentWealth;
    }

    private void OnDisable()
    {
        LStructure.OnAnyCapturedStructure       -= UpdatePlayerNetIncome;
        RecruitmentController.OnAnyPurchaseUnit -= UpdateCurrentWealth;
    }

    public void OnTurnStart(object sender, System.EventArgs e)
    {
        UpdateUpkeep(_cellGrid.CurrentPlayerNumber);
        UpdateEconomyIncome(_cellGrid.CurrentPlayerNumber);
        UpdateCurrentWealth(null,
                            _cellGrid.CurrentPlayerNumber,
                            AccountDictionary[_cellGrid.CurrentPlayerNumber].EconomyIncome);
    }


    private void OnGameStarted(object sender, System.EventArgs e)
    {
        foreach (var player in (sender as CellGrid).Players)
        {
            PlayerAccount account = new PlayerAccount();
            AccountDictionary.Add(player.PlayerNumber, account);
            UpdateUpkeep(player.PlayerNumber);
            UpdateEconomyIncome(player.PlayerNumber);
            int startingAmount = 0;
            if (player is HumanPlayer)
                startingAmount = _humanStartingAmount;
            else if (player is AIPlayer)
                startingAmount = _aiStartingAmount;
            UpdateCurrentWealth(null, player.PlayerNumber, startingAmount);
        }
    }

    public int GetCurrentWealth(int playerNumber)
    {
        if (AccountDictionary.TryGetValue(playerNumber, out var account))
            return account.Wealth;
        return 0;
    }

    public void UpdateCurrentWealth(LUnit newUnit, int playerNumber, int income = 0)
    {
        AccountDictionary[playerNumber].Wealth += income;

        if (AccountDictionary[playerNumber].Wealth < 0) AccountDictionary[playerNumber].Wealth = 0;

        if (playerNumber == 0) OnAnyUpdateWealth?.Invoke(AccountDictionary[playerNumber]);
    }

    public void UpdateUpkeep(int playerNumber)
    {
        if (CellGrid.Instance == null) return;
        var units = CellGrid.Instance.Units.FindAll(unit => unit.PlayerNumber == playerNumber &&
                                                            unit is not LStructure);

        int totalUnitUpkeep = 0;
        for (int i = 0; i < units.Count; i++)
            if (units[i] is LUnit lUnit)
                totalUnitUpkeep += lUnit.UnitStats.Upkeep;

        AccountDictionary[playerNumber].Upkeep = totalUnitUpkeep;
        if (playerNumber == 0) OnAnyUpkeepUpdated?.Invoke(totalUnitUpkeep);
    }

    private void UpdateEconomyIncome(int playerNumber)
    {
        if (CellGrid.Instance == null) return;
        var structures =
            CellGrid.Instance.Units.FindAll(unit => unit.PlayerNumber == playerNumber && unit is LStructure);

        int economyIncome = 0;
        for (int i = 0; i < structures.Count; i++)
            if (structures[i] is LStructure lStructure)
                if (lStructure.UnitStats is StructureStats structureStats)
                    economyIncome += structureStats.Income;


        economyIncome -= AccountDictionary[playerNumber].Upkeep;
        int defaultAIIncomeOnly = _defaultAIAmount;
        if (_cellGrid.CurrentPlayer is HumanPlayer)
            economyIncome += _defaultPlayerAmount;
        else if (_cellGrid.CurrentPlayer is AIPlayer)
            economyIncome += _defaultAIAmount;

        AccountDictionary[playerNumber].EconomyIncome = economyIncome;
        if (playerNumber == 0)
        {
            if (_cellGrid.CurrentPlayer is AIPlayer)
                OnAnyNetIncomeUpdated?.Invoke(economyIncome - defaultAIIncomeOnly);
            else
                OnAnyNetIncomeUpdated?.Invoke(economyIncome);
        }
    }

    private void UpdateUpkeepAndNetIncome()
    {
        UpdatePlayerUpkeep();
        UpdatePlayerNetIncome();
    }

    private void UpdatePlayerUpkeep()
    {
        if (AccountDictionary.Count == 0) return;
        UpdateUpkeep(0);
    }

    private void UpdatePlayerNetIncome()
    {
        if (AccountDictionary.Count == 0) return;
        UpdateEconomyIncome(0);
    }
}