using UnityEngine;

public class CellSpriteRandomizer : MonoBehaviour
{
    [SerializeField] private Sprite[] _spriteArray;

    private SpriteRenderer _spriteRenderer;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();

    private void Start() => RandomizeSprite();

    private void RandomizeSprite()
    {
        int spriteIndex = Random.Range(0, _spriteArray.Length);
        _spriteRenderer.sprite = _spriteArray[spriteIndex];
    }
}
