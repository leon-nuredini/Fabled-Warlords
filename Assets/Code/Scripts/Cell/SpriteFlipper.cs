using UnityEngine;

public class SpriteFlipper : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake() => _spriteRenderer = GetComponent<SpriteRenderer>();
    
    private void Start()
    {
        int randomX = Random.Range(0, 10);
        int randomY = Random.Range(0, 10);

        _spriteRenderer.flipX = randomX >= 5;
        _spriteRenderer.flipY = randomY >= 5;
    }
}