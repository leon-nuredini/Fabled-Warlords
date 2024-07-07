using UnityEngine;

[CreateAssetMenu(fileName = "ObjectiveDetails", menuName = "Objectives/Details", order = 0)]
public class ObjectiveDetails : ScriptableObject
{
    [SerializeField]                             private string _title;
    [TextAreaAttribute(15, 10)] [SerializeField] private string _description;

    public string Title => _title;
    public string Description => _description;
}
