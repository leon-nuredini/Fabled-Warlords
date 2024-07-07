using UnityEngine;

public class TutorialText : MonoBehaviour
{
    [SerializeField] private GameObject _moveUnitText;
    [SerializeField] private GameObject _defeatEnemyText;
    [SerializeField] private GameObject _captureVillageText;
    [SerializeField] private GameObject _recruitUnitsText;
    [SerializeField] private GameObject _inspectTerrainText;

    private int _unitsRecruited;

    private void Start() => DisableAllTextGameObjects();

    private void OnEnable()
    {
        UITutorial.OnAnyMoveUnit += DisplayMoveUnitText;
        UITutorial.OnAnyAllowEndingTurn += DisableAllTextGameObjects;
        UITutorial.OnAnyAttackEnemyUnit += DisplayDefeatEnemyText;
        UITutorial.OnAnyCaptureVillage += DisplayCaptureVillageText;
        UITutorial.OnAnyDisplayVillageDescription += DisableAllTextGameObjects;
        UITutorial.OnAnyInspectTile += DisplayInspectTerrainText;
        UIRecruitment.OnAnyClickRecruitButton += DisplayRecruitText;
        RecruitmentController.OnAnyPurchaseUnit += OnNewUnitRecruited;
        TerrainDescriptionPresenter.OnAnyOpenTerrainDescriptionPanel += DisableAllTextGameObjects;
    }

    private void OnDisable()
    {
        UITutorial.OnAnyMoveUnit -= DisplayMoveUnitText;
        UITutorial.OnAnyAllowEndingTurn -= DisableAllTextGameObjects;
        UITutorial.OnAnyAttackEnemyUnit -= DisplayDefeatEnemyText;
        UITutorial.OnAnyCaptureVillage -= DisplayCaptureVillageText;
        UITutorial.OnAnyDisplayVillageDescription -= DisableAllTextGameObjects;
        UITutorial.OnAnyInspectTile -= DisplayInspectTerrainText;
        UIRecruitment.OnAnyClickRecruitButton -= DisplayRecruitText;
        RecruitmentController.OnAnyPurchaseUnit -= OnNewUnitRecruited;
        TerrainDescriptionPresenter.OnAnyOpenTerrainDescriptionPanel -= DisableAllTextGameObjects;
    }

    private void DisplayMoveUnitText()
    {
        DisableAllTextGameObjects();
        _moveUnitText.SetActive(true);
    }

    private void DisplayDefeatEnemyText()
    {
        DisableAllTextGameObjects();
        _defeatEnemyText.SetActive(true);
    }

    private void DisplayCaptureVillageText()
    {
        DisableAllTextGameObjects();
        _captureVillageText.SetActive(true);
    }

    private void DisplayRecruitText()
    {
        if (_unitsRecruited >= 2) return;
        DisableAllTextGameObjects();
        _recruitUnitsText.SetActive(true);
    }

    private void DisplayInspectTerrainText()
    {
        DisableAllTextGameObjects();
        _inspectTerrainText.SetActive(true);
    }

    private void OnNewUnitRecruited(LUnit unit, int playerNumber, int cost)
    {
        if (playerNumber != 0) return;
        _unitsRecruited++;
        DisableAllTextGameObjects();
    }

    private void DisableAllTextGameObjects()
    {
        _moveUnitText.SetActive(false);
        _defeatEnemyText.SetActive(false);
        _captureVillageText.SetActive(false);
        _recruitUnitsText.SetActive(false);
        _inspectTerrainText.SetActive(false);
    }
}