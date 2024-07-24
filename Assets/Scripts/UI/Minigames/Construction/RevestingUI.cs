using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class RevestingUI : MonoBehaviour
    {
        [SerializeField] private Button _confirmButton;

        public event System.Action OnConfirm;


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
        }

        private void Finish()
        {
            _confirmButton.onClick.RemoveAllListeners();
        }


        private void CheckGrid()
        {
            OnConfirm?.Invoke();
        }
    }
}
