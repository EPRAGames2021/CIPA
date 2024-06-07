using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    [RequireComponent(typeof(ScreenTouchController))]
    [RequireComponent(typeof(MouseDelta))]
    public class FoundationAndStructureUI : MonoBehaviour
    {
        [Header("Dev area")]
        [SerializeField] private ScreenTouchController _screenTouchController;

        [SerializeField] private IngredientSelectionPanel _formSelectionPanel;
        [SerializeField] private IngredientSelectionPanel _concreteSelectionPanel;
        [SerializeField] private ConcreteMixPanel _concreteMixPanel;
        [SerializeField] private PouringPanel _pouringPanel;

        [SerializeField] private int _stageIndex;

        [SerializeField] private List<GameObject> _stagePanels;

        public int StageIndex => _stageIndex;


        public event System.Action OnMinigameFailed;

        private void OnEnable()
        {
            Refresh();
        }

        private void Start()
        {
            Init();
        }

        private void OnDestroy()
        {
            Finish();
        }

        private void Refresh()
        {
            _stageIndex = 0;
            EnablePanel(_stageIndex);
        }

        private void Init()
        {
            Refresh();

            _formSelectionPanel.OnIngredientsAreCorrect += CheckAdvanceStage;
            _concreteSelectionPanel.OnIngredientsAreCorrect += CheckAdvanceStage;
            _concreteMixPanel.OnMixSucceeded += CheckAdvanceStage;
            _pouringPanel.OnPouringSucceeded += CheckAdvanceStage;
        }

        private void Finish()
        {
            _formSelectionPanel.OnIngredientsAreCorrect -= CheckAdvanceStage;
            _concreteSelectionPanel.OnIngredientsAreCorrect -= CheckAdvanceStage;
            _concreteMixPanel.OnMixSucceeded -= CheckAdvanceStage;
            _pouringPanel.OnPouringSucceeded -= CheckAdvanceStage;
        }


        private void EnablePanel(int index)
        {
            for (int i = 0; i < _stagePanels.Count; i++)
            {
                _stagePanels[i].SetActive(i == index);
            }
        }

        private void NextPanel()
        {
            _stageIndex++;
            EnablePanel(_stageIndex);
        }

        private void CheckAdvanceStage(bool canAdvance)
        {
            if (canAdvance)
            {
                if (_stageIndex < _stagePanels.Count - 1)
                {
                    NextPanel();

                    MissionManager.Instance.GoToNextMission();

                    _screenTouchController.ReInit();
                }
                else
                {
                    JobAreaManager.Instance.MinigameSuccessed();

                    gameObject.SetActive(false);
                }
            }
            else
            {
                JobAreaManager.Instance.MinigameFailed();

                OnMinigameFailed?.Invoke();

                MissionManager.Instance.GoToMission(2);

                gameObject.SetActive(false);
            }
        }
    }
}
