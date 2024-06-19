using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class PlateAndCoilProductionUI : MonoBehaviour
    {
        [SerializeField] private Button _confirmButton;

        [SerializeField] private PlateAndCoilGrid _plateAndCoilGrid;

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
            _confirmButton.onClick.AddListener(CheckGrid);

            _plateAndCoilGrid.OnContainerOverflow += CheckGrid;
        }

        private void Finish()
        {
            _confirmButton.onClick.RemoveAllListeners();

            _plateAndCoilGrid.OnContainerOverflow -= CheckGrid;
        }

        private void CheckGrid()
        {
            JobAreaManager.Instance.FinishMinigame(_plateAndCoilGrid.CheckForCompletion());

            _plateAndCoilGrid.Filling = false;

            gameObject.SetActive(false);
        }
    }
}
