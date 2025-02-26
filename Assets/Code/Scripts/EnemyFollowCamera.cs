using TbsFramework.Grid;
using TbsFramework.Players;
using TbsFramework.Units.Abilities;
using UnityEngine;

public class EnemyFollowCamera : MonoBehaviour
{
    private Transform _cameraTransform;

    private Vector3 _originalCameraPosition;

    private void Awake() => _cameraTransform = Camera.main.transform;

    private void OnEnable()
    {
        if (CellGrid.Instance != null)
            CellGrid.Instance.TurnEnded += UpdateCameraPosition;
        MoveAbility.OnAnyMoveAbilityTriggered += CenterCameraAtPosition;
        AttackAbility.OnAnyAttackAbilityTriggered += CenterCameraAtPosition;
    }

    private void OnDisable()
    {
        if (CellGrid.Instance != null)
            CellGrid.Instance.TurnEnded -= UpdateCameraPosition;
        MoveAbility.OnAnyMoveAbilityTriggered -= CenterCameraAtPosition;
        AttackAbility.OnAnyAttackAbilityTriggered -= CenterCameraAtPosition;
    }

    private void UpdateCameraPosition(object o, bool eventVal)
    {
        if (CellGrid.Instance.CurrentPlayer is HumanPlayer)
        {
            CenterCameraAtPosition(_originalCameraPosition);
        }
        else if (CellGrid.Instance.CurrentPlayer is AIPlayer)
        {
            _originalCameraPosition.x = _cameraTransform.position.x;
            _originalCameraPosition.y = _cameraTransform.position.y;
        }
    }

    private void CenterCameraAtPosition(Vector3 position)
    {
        if (GameSettings.Instance != null && !GameSettings.Instance.Preferences.EnableCameraTracking) return;

        Vector3 newCameraPosition = _cameraTransform.position;
        newCameraPosition.x = position.x;
        newCameraPosition.y = position.y;
        _cameraTransform.position = newCameraPosition;
    }
}