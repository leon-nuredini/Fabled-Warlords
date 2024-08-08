using System;
using NaughtyAttributes;
using UnityEngine;

public class RangeFinderAnimatorController : MonoBehaviour
{
    [Header("Cell Range Visuals")]
    
    [HorizontalLine(color: EColor.Red)]
    [SerializeField] private Sprite red;
    
    [HorizontalLine(color: EColor.Green)]
    [SerializeField] private Sprite green; 
    
    [HorizontalLine(color: EColor.Blue)]
    [SerializeField] private Sprite blue;
    
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeCellSprite(RangeFinderType rangeFinderType)
    {
        _spriteRenderer.sprite = rangeFinderType switch
        {
            RangeFinderType.None => null,
            RangeFinderType.Red => red,
            RangeFinderType.Green => green,
            RangeFinderType.Blue => blue,
            _ => _spriteRenderer.sprite
        };
    }
}