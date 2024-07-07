using UnityEngine;

[CreateAssetMenu(fileName = "TutorialData", menuName = "TutorialData/TutorialData", order = 0)]
public class TutorialData : ScriptableObject
{
    [SerializeField]                             private string _title;
    [SerializeField] [TextAreaAttribute(15, 10)] private string _description;
    [SerializeField] [TextAreaAttribute(15, 10)] private string _objectiveDescription;

    public string Title       => _title;
    public string Description => _description;

    public string ObjectiveDescription => _objectiveDescription;
}