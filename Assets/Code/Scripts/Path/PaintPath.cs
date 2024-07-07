using NaughtyAttributes;
using UnityEngine;

public class PaintPath : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    [SerializeField] private PathType     _pathType = PathType.None;
    [SerializeField] private PathGraphics _redPathGraphics;

    [BoxGroup("Colors")] [SerializeField] private Color _defaultColor;
    [BoxGroup("Colors")] [SerializeField] private Color _fadedColor;

    public PathType PathType { get => _pathType; set => _pathType = value; }

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        DisableSpritePath();
    }

    public void DrawPath(PathType pathType)
    {
        _pathType = pathType;
        switch (_pathType)
        {
            case PathType.None:
                DisableSpritePath();
                break;
            case PathType.Up:
                UpdatePathSprite(_redPathGraphics.MoveUpOnceSprite);
                break;
            case PathType.Down:
                UpdatePathSprite(_redPathGraphics.MoveDownOnceSprite);
                break;
            case PathType.Left:
                UpdatePathSprite(_redPathGraphics.MoveLeftOnceSprite);
                break;
            case PathType.Right:
                UpdatePathSprite(_redPathGraphics.MoveRightOnceSprite);
                break;
        }
    }

    private void UpdatePathSprite(Sprite pathSprite)
    {
        _spriteRenderer.color  = _defaultColor;
        _spriteRenderer.sprite = pathSprite;
        _spriteRenderer.gameObject.SetActive(true);
    }

    public void DisableSpritePath()
    {
        _spriteRenderer.color  = _fadedColor;
        _spriteRenderer.sprite = null;
        _spriteRenderer.gameObject.SetActive(false);
    }
}