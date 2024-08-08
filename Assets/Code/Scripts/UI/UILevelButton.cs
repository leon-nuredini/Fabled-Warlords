using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UILevelButton : MonoBehaviour
{
    public event Action<UILevelButton> OnLevelSelected;

    [SerializeField] private LevelDetails _levelDetails;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private GameObject[] _elements;
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Color _disabledTextColor;

    private Button _button;
    private RectTransform _rectTransform;

    #region Properties

    public LevelDetails LevelDetails => _levelDetails;
    public Button Button => _button;
    public RectTransform RectTransform => _rectTransform;

    #endregion

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelectLevelButton);
        _titleText.text = LevelDetails.LevelName;
        _iconImage.sprite = _levelDetails.LevelIcon;
        _rectTransform = GetComponent<RectTransform>();
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
        int levelIndex = _levelDetails.LevelIndex;
        _iconImage.sprite = _levelDetails.LevelIcon;
        string prefix = ToRoman(levelIndex) + ": ";
        _text.text = $"{prefix} Locked";
        _text.color = _disabledTextColor;
    }

    public string ToRoman(int number)
    {
        if ((number < 0) || (number > 3999))
        {
            Debug.LogWarning("Input a number between 1 and 3999");
            return "ERROR";
        }

        if (number < 1) return string.Empty;
        if (number >= 1000) return "M" + ToRoman(number - 1000);
        if (number >= 900) return "CM" + ToRoman(number - 900);
        if (number >= 500) return "D" + ToRoman(number - 500);
        if (number >= 400) return "CD" + ToRoman(number - 400);
        if (number >= 100) return "C" + ToRoman(number - 100);
        if (number >= 90) return "XC" + ToRoman(number - 90);
        if (number >= 50) return "L" + ToRoman(number - 50);
        if (number >= 40) return "XL" + ToRoman(number - 40);
        if (number >= 10) return "X" + ToRoman(number - 10);
        if (number >= 9) return "IX" + ToRoman(number - 9);
        if (number >= 5) return "V" + ToRoman(number - 5);
        if (number >= 4) return "IV" + ToRoman(number - 4);
        if (number >= 1) return "I" + ToRoman(number - 1);
        return string.Empty;
    }
}