using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenTouchController))]
public class HydraulicsUI : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;

    [SerializeField] private PipeGrid _pipeGrid;

    [Header("Touch handler")]
    [SerializeField] private ScreenTouchController _screenTouchController;


    private void OnValidate()
    {
        if (_screenTouchController == null)
        {
            _screenTouchController = GetComponent<ScreenTouchController>();
        }
    }

    private void OnEnable()
    {
        Camera.main.orthographic = true;
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
        //JobAreaManager.Instance.MinigameSuccessed();
        _pipeGrid.CheckForCorrectGrid();

        //gameObject.SetActive(false);
    }
}
