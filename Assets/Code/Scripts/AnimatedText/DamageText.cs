using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _damageText;

    public void UpdateTextValue(string damageText) => _damageText.text = damageText;
}