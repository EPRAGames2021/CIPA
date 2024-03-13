using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ElaborateTranslateExample : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    [SerializeField] private int _score;

    private void OnValidate()
    {
        UpdateScore();
    }


    private void UpdateScore()
    {
        _scoreText.text = LanguageManager.GetTranslation("scoreDisplay", _score);
    }
}
