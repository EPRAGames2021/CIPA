using UnityEngine;

namespace CIPA
{
    [RequireComponent(typeof(ScreenTouchController))]
    public class LandGradingUI : MonoBehaviour
    {
        [Header("Dev area")]
        [SerializeField] private PouringPanel _pouringPanel;

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
            _pouringPanel.OnPouringSucceeded += FinishMinigame;
        }

        private void Finish()
        {
            _pouringPanel.OnPouringSucceeded -= FinishMinigame;
        }

        private void FinishMinigame(bool succeeded)
        {
            if (succeeded)
            {
                JobAreaManager.Instance.MinigameSuccessed();
            }
            else
            {
                JobAreaManager.Instance.MinigameFailed();
            }

            gameObject.SetActive(false);
        }
    }
}
