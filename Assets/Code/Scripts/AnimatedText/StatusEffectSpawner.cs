using System.Collections;
using System.Collections.Generic;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class StatusEffectSpawner : MonoBehaviour
{
    private Queue<StatusEffectText> _textQueue = new Queue<StatusEffectText>();

    private Coroutine _coroutine;

    private readonly string _outlineColor = "_OutlineColor";

    public void QueueStatusTextEffect(LUnit lUnit, TextMeshPro text, Vector3 position, float textSpawnDelay,
        Color color)
    {
        StatusEffectText statusEffectText = new StatusEffectText(lUnit, text, position, textSpawnDelay, color);
        _textQueue.Enqueue(statusEffectText);

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(SpawnQueuedTextEffects());
    }

    private IEnumerator SpawnQueuedTextEffects()
    {
        while (_textQueue.Count > 0)
        {
            StatusEffectText queuedStatusEffectText = _textQueue.Peek();
            yield return new WaitForSeconds(queuedStatusEffectText.TextSpawnDelay);
            _textQueue.Dequeue();
            if (queuedStatusEffectText.LUnit == null || queuedStatusEffectText.LUnit.HitPoints <= 0) continue;
            GameObject spawnedTextGobj = LeanPool.Spawn(queuedStatusEffectText.TextMeshPro.gameObject,
                queuedStatusEffectText.SpawnPosition,
                Quaternion.identity);

            MeshRenderer textMeshRenderer = spawnedTextGobj.GetComponent<MeshRenderer>();
            Material textMaterial = textMeshRenderer.material;
            textMaterial.SetColor(_outlineColor, queuedStatusEffectText.OutlineColor);
        }
    }
}

public class StatusEffectText
{
    private LUnit _lUnit;
    private TextMeshPro _textMeshPro;
    private Vector3 _spawnPosition;
    private float _textSpawnDelay;
    private Color _outlineColor;

    public StatusEffectText(LUnit lUnit, TextMeshPro text, Vector3 position, float textSpawnDelay, Color color)
    {
        _lUnit = lUnit;
        _textMeshPro = text;
        _spawnPosition = position;
        _textSpawnDelay = textSpawnDelay;
        _outlineColor = color;
    }

    public TextMeshPro TextMeshPro => _textMeshPro;
    public Vector3 SpawnPosition => _spawnPosition;
    public LUnit LUnit => _lUnit;
    public float TextSpawnDelay => _textSpawnDelay;
    public Color OutlineColor => _outlineColor;
}