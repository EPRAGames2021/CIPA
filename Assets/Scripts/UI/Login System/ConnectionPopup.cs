using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using EPRA.Utilities;
using TMPro;

public class ConnectionPopup : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _connectionStatusText;
    [SerializeField] private Button _tryAgain;

    [SerializeField] private Coroutine _tryAgainCoroutine;
    [SerializeField] private int _maxAttempts;
    [SerializeField] private int _attemptLeft;

    private void Start()
    {
        Init();

        AttemptConnection();
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void Init()
    {
        _attemptLeft = _maxAttempts;

        _tryAgain.gameObject.SetActive(false);
        _tryAgain.onClick.AddListener(TryAgain);

        _tryAgainCoroutine = null;
    }

    private void Finish()
    {
        _tryAgain.onClick.RemoveAllListeners();
    }

    private void AttemptConnection()
    {
        if (FirebaseHandler.Instance == null)
        {
            gameObject.SetActive(false);

            return;
        }

        _connectionStatusText.text = "Checking connection";

        if (FirebaseHandler.Instance.IsConnected)
        {
            CanvasManager.Instance.OpenMenu(MenuType.LoginMenu);

            Debug.LogWarning("Connection to Firebase successful");

            StopCoroutine(_tryAgainCoroutine);
            _tryAgainCoroutine = null;

            gameObject.SetActive(false);
        }
        else
        {
            _tryAgain.gameObject.SetActive(true);

            _connectionStatusText.text = "Connection not successful";

            Debug.LogWarning("Connection to Firebase not successful");

            if (_tryAgainCoroutine == null)
            {
                _tryAgainCoroutine = StartCoroutine(ReattemptConnection());
            }            
        }
    }

    private IEnumerator ReattemptConnection()
    {
        _attemptLeft--;

        Debug.LogWarning("Initiating a new attempt to connect to Firebase. Attemps left: " + _attemptLeft);

        yield return new WaitForSeconds(0.5f);

        if (_maxAttempts > 0)
        {
            AttemptConnection();
        }
        else
        {
            _attemptLeft = _maxAttempts;

            TryAgain();
        }
    }


    private void TryAgain()
    {
        _tryAgain.gameObject.SetActive(false);

        AttemptConnection();
    }
}
