using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreenContainer;
    [SerializeField] private Slider _slider;
    [SerializeField] private TextMeshProUGUI _percentage;
    [SerializeField] private TextMeshProUGUI _loading;

    private string _loadingString;


    private void Start()
    {
        InitializeVariables();
        SubscribeToEvents();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }


    private void InitializeVariables()
    {
        _percentage.text = "0%";

        _slider.minValue = 0;
        _slider.maxValue = 100;
        _slider.value = 0;
    }

    private void SubscribeToEvents()
    {
        SceneLoader.Instance.OnLoadIsInProgress += DisplayLoadingScreen;
        SceneLoader.Instance.OnProgressChanges += SetPercentage;
    }

    private void UnsubscribeFromEvents()
    {
        SceneLoader.Instance.OnLoadIsInProgress -= DisplayLoadingScreen;
        SceneLoader.Instance.OnProgressChanges -= SetPercentage;
    }


    private void SetPercentage(float progress)
    {
        _slider.value = progress;

        _percentage.text = progress.ToString() + "%";
    }

    private void DisplayLoadingScreen(bool display)
    {
        if (display) _loadingScreenContainer.SetActive(true);
        else StartCoroutine(DisableDelay());
    }

    private IEnumerator DisableDelay()
    {
        yield return new WaitForEndOfFrame();

        _loadingScreenContainer.SetActive(false);
    }


    public void Activate()
    {
        _loadingScreenContainer.SetActive(true);
    }
}
