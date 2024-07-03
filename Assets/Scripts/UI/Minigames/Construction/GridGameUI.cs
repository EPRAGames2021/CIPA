using UnityEngine;
using UnityEngine.UI;

namespace CIPA
{
    public class GridGameUI : MonoBehaviour
    {
        [SerializeField] private Button _confirmButton;

        [SerializeField] private GridGameController _controller;

        private void OnEnable()
        {
            Init();
        }

        private void OnDisable()
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
            JobAreaManager.Instance.FinishMinigame(_controller.CheckIfGridIsCorrect());

            gameObject.SetActive(false);
        }
    }
}
