using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UILevelButton : MonoBehaviour
{
    public event Action<UILevelButton> OnLevelSelected;

    [SerializeField] private LevelDetails    _levelDetails;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject[]    _elements;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Color           _disabledTextColor;

    private Button _button;

    public LevelDetails LevelDetails => _levelDetails;
    public Button Button => _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectLevelButton);
        _titleText.text = LevelDetails.LevelName;
    }

    public void SelectLevelButton()
    {
        for (int i = 0; i < _elements.Length; i++)
            _elements[i].SetActive(true);

        OnLevelSelected?.Invoke(this);
    }

    public void DeselectLevelButton()
    {
        for (int i = 0; i < _elements.Length; i++)
            _elements[i].SetActive(false);
    }

    public void Lock()
    {
        _button.interactable = false;
        int    levelIndex = _levelDetails.LevelIndex;
        string prefix     = String.Empty;
        switch (levelIndex)
        {
            case 2:
                prefix = "II: ";
                break;
            case 3:
                prefix = "III: ";
                break;
            case 4:
                prefix = "IV: ";
                break;
            case 5:
                prefix = "V: ";
                break;
            case 6:
                prefix = "VI: ";
                break;
        }

        _text.text  = $"{prefix} Locked";
        _text.color = _disabledTextColor;
    }
}