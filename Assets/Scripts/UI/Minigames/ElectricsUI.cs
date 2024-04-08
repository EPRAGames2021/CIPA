using UnityEngine;
using UnityEngine.UI;

public class ElectricsUI : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;

    [SerializeField] private PipeGrid _wireGrid;

    private void OnEnable()
    {
        Camera.main.orthographic = true;

        _wireGrid.ResetGrid();
    }

    private void Start()
    {
        Init();
    }

    private void OnDisable()
    {
        Camera.main.orthographic = false;
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void Init()
    {
        _confirmButton.onClick.AddListener(CheckPipes);
    }

    private void Finish()
    {
        _confirmButton.onClick.RemoveAllListeners();
    }


    private void CheckPipes()
    {
        if (_wireGrid.CheckForCorrectGrid())
        {
            JobAreaManager.Instance.MinigameSuccessed();

            _wireGrid.LockGrid();

            gameObject.SetActive(false);
        }
        else
        {
            JobAreaManager.Instance.MinigameFailed();

            _wireGrid.LockGrid();

            gameObject.SetActive(false);
        }
    }
}
