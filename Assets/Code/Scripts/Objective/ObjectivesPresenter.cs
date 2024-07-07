using TMPro;
using UnityEngine;

public class ObjectivesPresenter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private ObjectiveDetails _objectiveDetails;

    private void Awake()
    {
        _titleText.text       = _objectiveDetails.Title;
        _descriptionText.text = _objectiveDetails.Description;
    }
}
