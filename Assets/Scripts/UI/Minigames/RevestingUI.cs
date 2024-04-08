using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class RevestingUI : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;

    [SerializeField] private TileGrid _tileGrid;
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private void OnEnable()
    {
        _virtualCamera.m_Lens.Orthographic = true;
        _virtualCamera.m_Lens.OrthographicSize = 25f;

        _tileGrid.ResetGrid();
    }

    private void Start()
    {
        Init();
    }

    private void OnDisable()
    {
        _virtualCamera.m_Lens.OrthographicSize = 5;
        _virtualCamera.m_Lens.Orthographic = true;
    }

    private void OnDestroy()
    {
        Finish();
    }


    private void Init()
    {
        _confirmButton.onClick.AddListener(CheckGrid);
    }

    private void Finish()
    {
        _confirmButton.onClick.RemoveAllListeners();
    }


    private void CheckGrid()
    {
        if (_tileGrid.CheckForCorrectGrid())
        {
            JobAreaManager.Instance.MinigameSuccessed();

            _tileGrid.LockGrid();

            gameObject.SetActive(false);
        }
        else
        {
            JobAreaManager.Instance.MinigameFailed();

            _tileGrid.LockGrid();

            gameObject.SetActive(false);
        }
    }
}
