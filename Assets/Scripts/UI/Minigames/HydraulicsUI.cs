using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScreenTouchController))]
public class HydraulicsUI : MonoBehaviour
{
    [SerializeField] private Button _confirmButton;

    [Header("Touch handler")]
    [SerializeField] private ScreenTouchController _screenTouchController;


    private void OnValidate()
    {
        if (_screenTouchController == null)
        {
            _screenTouchController = GetComponent<ScreenTouchController>();
        }
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
        //_confirmButton.onClick.AddListener(CheckPipes);
    }

    private void Finish()
    {
        //_confirmButton.onClick.RemoveAllListeners();
    }


    private void CheckPipes()
    {

    }
}
