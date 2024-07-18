using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

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


    public bool IsBeingDisplayed => _loadingScreenContainer.activeInHierarchy;

    public static event System.Action OnScreenHasBeenClosed;


    private void OnEnable()
    {
        AttemptToPickTip();
    }

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

        _continueButton.gameObject.SetActive(!_autoHide);
        //_continueButton.onClick.AddListener(() => _loadingScreenContainer.SetActive(false));
        _continueButton.onClick.AddListener(CloseLoadingScreen);
    }

    private void Finish()
    {
        _continueButton.onClick.RemoveAllListeners();
    }


    private void CloseLoadingScreen()
    {
        _loadingScreenContainer.SetActive(false);

        OnScreenHasBeenClosed?.Invoke();
    }

    public void SetPercentage(float progress)
    {
        int progressInteger = Mathf.RoundToInt(progress);

        _slider.value = progressInteger;
        _percentage.text = progressInteger.ToString() + "%";

        _loading.gameObject.SetActive(progressInteger <= 100);
        _slider.gameObject.SetActive(progressInteger <= 100);
        _percentage.gameObject.SetActive(progressInteger <= 100);
    }

    public void DisplayLoadingScreen(bool loadIsInProgress)
    {
        if (loadIsInProgress)
        {
            AttemptToPickTip();

            _continueButton.interactable = false;

            _loading.gameObject.SetActive(true);
            _slider.gameObject.SetActive(true);
            _percentage.gameObject.SetActive(true);

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
                _loading.gameObject.SetActive(false);
                _slider.gameObject.SetActive(false);
                _percentage.gameObject.SetActive(false);

                _continueButton.interactable = true;
            }
        }
    }


    private void AttemptToPickTip()
    {
        if (LanguageManager.LanguagesLoaded)
        {
            PickRandomTip();
        }
        else
        {
            StartCoroutine(PickRandomTipDelayed());
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


    public void SelectUI()
    {
        _continueButton.Select();
    }
}
