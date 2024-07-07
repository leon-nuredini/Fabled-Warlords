using TMPro;
using UnityEngine;

public class HealText : MonoBehaviour
{
    [SerializeField] private TextMeshPro _healText;

    public void UpdateTextValue(string damageText) => _healText.text = damageText;
}
