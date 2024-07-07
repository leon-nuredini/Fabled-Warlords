using System.Linq;
using NaughtyAttributes;
using TbsFramework.Grid;
using TMPro;
using UnityEngine;

public class ResourcesPresenter : MonoBehaviour
{
    [BoxGroup("Resource Text")] [SerializeField]
    private TextMeshProUGUI _turnNumberText;

    [BoxGroup("Resource Text")] [SerializeField]
    private TextMeshProUGUI _wealthText;

    [BoxGroup("Resource Text")] [SerializeField]
    private TextMeshProUGUI _structuresText;

    [BoxGroup("Resource Text")] [SerializeField]
    private TextMeshProUGUI _unitsText;

    [BoxGroup("Resource Text")] [SerializeField]
    private TextMeshProUGUI _upkeepText;

    [BoxGroup("Resource Text")] [SerializeField]
    private TextMeshProUGUI _netIncomeText;

    private void Start() => UpdateStructureCount();

    private void OnEnable()
    {
        EconomyController.OnAnyUpdateWealth     += UpdateWealth;
        EconomyController.OnAnyUpkeepUpdated    += UpdateUnitUpkeep;
        EconomyController.OnAnyNetIncomeUpdated += UpdateNetIncome;
        LStructure.OnAnyCapturedStructure       += UpdateStructureCount;
        if (CellGrid.Instance != null)
        {
            CellGrid.Instance.OnTurnNumberIncreased += UpdateTurnNumber;
            CellGrid.Instance.OnUnitAdded           += UpdateUnitCount;
            CellGrid.Instance.OnUnitRemoved         += UpdateUnitCount;
        }
    }

    private void OnDisable()
    {
        EconomyController.OnAnyUpdateWealth     -= UpdateWealth;
        EconomyController.OnAnyUpkeepUpdated    -= UpdateUnitUpkeep;
        EconomyController.OnAnyNetIncomeUpdated -= UpdateNetIncome;
        LStructure.OnAnyCapturedStructure       -= UpdateStructureCount;
        if (CellGrid.Instance != null)
        {
            CellGrid.Instance.OnTurnNumberIncreased -= UpdateTurnNumber;
            CellGrid.Instance.OnUnitAdded           -= UpdateUnitCount;
            CellGrid.Instance.OnUnitRemoved         -= UpdateUnitCount;
        }
    }

    private void UpdateTurnNumber(int       turnNumber) => _turnNumberText.text = turnNumber.ToString();
    private void UpdateWealth(PlayerAccount account)    => _wealthText.text = account.Wealth.ToString();

    private void UpdateStructureCount()
    {
        if (CellGrid.Instance == null) return;
        var structures           = CellGrid.Instance.Units.FindAll(unit => unit is LStructure);
        int totalStructureNumber = structures.Count(unit => unit is LStructure);
        int playerStructures     = structures.Count(unit => unit is LStructure && unit.PlayerNumber == 0);
        _structuresText.text = $"{playerStructures}/{totalStructureNumber}";
    }

    private void UpdateUnitCount()
    {
        int totalPlayerUnits = CellGrid.Instance.Units.Count(unit => unit.PlayerNumber == 0 && unit is not LStructure);
        _unitsText.text = totalPlayerUnits.ToString();
    }

    private void UpdateUnitUpkeep(int totalUpkeep)      => _upkeepText.text = totalUpkeep.ToString();

    private void UpdateNetIncome(int currentNetIncome)
    {
        if (currentNetIncome < 0) currentNetIncome = 0;
        _netIncomeText.text = currentNetIncome.ToString();
    } 
}