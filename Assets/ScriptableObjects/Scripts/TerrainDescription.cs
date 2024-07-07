using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "TerrainDescription", menuName = "TerrainDescription/Terrain", order = 0)]
public class TerrainDescription : ScriptableObject
{
    [SerializeField] private string _name;
    [TextArea][SerializeField] private string _description;
    [TextArea][SerializeField] private string _effect;
    [SerializeField]           private string    _movementCost;
    [TextArea][SerializeField] private string _strategicTip;

    [BoxGroup("Image")] [SerializeField] private Sprite _backgroundSprite;
    [BoxGroup("Image")] [SerializeField] private Sprite _foregroundSprite;

    public string Name         => $"{_name}";
    public string Description  => $"<b>Description:</b> {_description}";
    public string Effect       => $"<b>Effect:</b> {_effect}";
    public string MovementCost => $"<b>Movement Cost:</b> {_movementCost}";
    public string StrategicTip => $"<b>Strategic Tip:</b> {_strategicTip}";
    public Sprite BackgroundSprite => _backgroundSprite;
    public Sprite ForegroundSprite => _foregroundSprite;
}