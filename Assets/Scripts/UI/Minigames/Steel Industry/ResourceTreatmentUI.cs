using UnityEngine;

namespace CIPA
{
    public class ResourceTreatmentUI : MonoBehaviour
    {
        [Header("Dev area")]
        [SerializeField] private ScreenTouchController _screenTouchController;

        [SerializeField] private ConcreteMixPanel _mixPanel;


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
            _mixPanel.OnMixSucceeded += EndMiniGame;
        }

        private void Finish()
        {
            _mixPanel.OnMixSucceeded -= EndMiniGame;
        }


        private void EndMiniGame(bool succeeded)
        {
            JobAreaManager.Instance.FinishMinigame(succeeded);
        }
    }
}