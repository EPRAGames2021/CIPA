using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class HydraulicsUI : MonoBehaviour
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
            _confirmButton.onClick.AddListener(CheckPipes);
        }

        private void Finish()
        {
            _confirmButton.onClick.RemoveAllListeners();
        }


        private void CheckPipes()
        {
            if (_pipeGrid.CheckForCorrectGrid())
            {
                JobAreaManager.Instance.MinigameSuccessed();

                _pipeGrid.LockGrid();

                gameObject.SetActive(false);
            }
            else
            {
                JobAreaManager.Instance.MinigameFailed();

                _pipeGrid.LockGrid();

                gameObject.SetActive(false);
            }
        }
    }
}
