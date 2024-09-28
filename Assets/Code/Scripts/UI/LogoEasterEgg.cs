using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoEasterEgg : MonoBehaviour
{
    [SerializeField] private int _clickCounter = 5;

    [SerializeField] private TextLogoShineAnimation[] _textLogoShineAnimArray;

    private int _clickedAmount;

    public void TryToShineText()
    {
        _clickedAmount++;
        if (_clickedAmount == _clickCounter)
        {
            _clickedAmount = 0;
            for (int i = 0; i < _textLogoShineAnimArray.Length; i++)
                _textLogoShineAnimArray[i].InitAnimation();
        }
    }
}