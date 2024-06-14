using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class PlateAndCoilProductionUI : MonoBehaviour
    {
        [SerializeField] private Button _confirmButton;

        [SerializeField] private PipeGrid _pipeGrid;

        private void OnEnable()
        {
            Camera.main.orthographic = true;

            _pipeGrid.ResetGrid();
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
            _confirmButton.onClick.AddListener(CheckGrid);
        }

        private void Finish()
        {
            _confirmButton.onClick.RemoveAllListeners();
        }


        private void CheckGrid()
        {
            JobAreaManager.Instance.FinishMinigame(_pipeGrid.CheckForCorrectGrid());

            _pipeGrid.LockGrid();

            gameObject.SetActive(false);
        }
    }
}
