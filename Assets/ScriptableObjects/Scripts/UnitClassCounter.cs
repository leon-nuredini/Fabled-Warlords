using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitClassCounter", menuName = "UnitClassCounter/UnitClassCounter", order = 0)]
public class UnitClassCounter : ScriptableObject
{
    [BoxGroup("Unit Type Sprite")] [SerializeField]
    private Sprite _unitTypeSprite;

    [SerializeField] private float _vsSwordInfantryCounter = 1f;
    [SerializeField] private float _vsSpearInfantryCounter = 1f;
    [SerializeField] private float _vsRangedFactor = 1f;
    [SerializeField] private float _vsCavalryFactor = 1f;
    [SerializeField] private float _vsMageFactor = 1f;
    [SerializeField] private float _vsMonsterFactor = 1f;

    public float VSSwordInfantryCounter => _vsSwordInfantryCounter;
    public float VSSpearInfantryCounter => _vsSpearInfantryCounter;
    public float VSRangedFactor => _vsRangedFactor;
    public float VSCavalryFactor => _vsCavalryFactor;
    public float VSMageFactor => _vsMageFactor;
    public float VSMonsterFactor => _vsMonsterFactor;
    public Sprite UnitTypeSprite => _unitTypeSprite;
}