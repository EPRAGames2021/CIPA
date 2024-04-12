using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;
using ES3Types;

public class LoadingScreen : MonoBehaviour
{
    [Header("GD Area")]
    [Tooltip("Should the loading screen disappear automatically once scene is loaded?")]
    [SerializeField] private bool _autoHide;

    [Header("Dev Area")]
    [SerializeField] private GameObject _loadingScreenContainer;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _percentage;
    [SerializeField] private TextMeshProUGUI _loading;

    [SerializeField] private Button _continueButton;

    // This isn't ideal but I don't have time to rework this right now. Ideally, this could be parsed from the text document itself.
    [SerializeField] private int _tipsAmount;
    [SerializeField] private TextMeshProUGUI _tips;


    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void Init()
    {
        _percentage.text = "0%";

        _slider.minValue = 0;
        _slider.maxValue = 100;
        _slider.value = 0;

        SceneLoader.Instance.OnLoadIsInProgress += DisplayLoadingScreen;
        SceneLoader.Instance.OnProgressChanges += SetPercentage;

        _continueButton.gameObject.SetActive(!_autoHide);
        _continueButton.onClick.AddListener(() => _loadingScreenContainer.SetActive(false));
    }

    private void Finish()
    {
        SceneLoader.Instance.OnLoadIsInProgress -= DisplayLoadingScreen;
        SceneLoader.Instance.OnProgressChanges -= SetPercentage;

        _continueButton.gameObject.SetActive(!_autoHide);
        _continueButton.onClick.RemoveAllListeners();
    }


    private void SetPercentage(float progress)
    {
        int progressInteger = Mathf.RoundToInt(progress);

        _slider.value = progressInteger;
        _percentage.text = progressInteger.ToString() + "%";

        _loading.gameObject.SetActive(progressInteger < 100);
        _slider.gameObject.SetActive(progressInteger < 100);
        _percentage.gameObject.SetActive(progressInteger < 100);
    }

    private void DisplayLoadingScreen(bool display)
    {
        if (display)
        {
            if (LanguageManager.LanguagesLoaded)
            {
                PickRandomTip();
            }
            else
            {
                StartCoroutine(PickRandomTipDelayed());
            }

            _continueButton.interactable = false;

            _loadingScreenContainer.SetActive(true);
        }
        else
        {
            if (_autoHide)
            {
                StartCoroutine(DisableDelay());
            }
            else
            {
                _continueButton.interactable = true;
            }
        }
    }

    private IEnumerator DisableDelay()
    {
        yield return new WaitForEndOfFrame();

        _loadingScreenContainer.SetActive(false);
    }

    private IEnumerator PickRandomTipDelayed()
    {
        _tips.text = string.Empty;

        yield return new WaitUntil(() => LanguageManager.LanguagesLoaded);

        PickRandomTip();
    }

    private void PickRandomTip()
    {
        int random = Random.Range(0, _tipsAmount);
        string key = "tip" + random;

        _tips.text = LanguageManager.GetTranslation(key);
    }
}
