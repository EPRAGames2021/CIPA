using UnityEngine;

[RequireComponent(typeof(ScreenTouchController))]
[RequireComponent(typeof(MouseDelta))]
public class FoundationAndStructureUI : MonoBehaviour
{
    [Header("Dev area")]
    [SerializeField] private IngredientSelectionPanel _ingredientSelectionPanel;
    [SerializeField] private ConcreteMixPanel _concreteMixPanel;

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
        _ingredientSelectionPanel.gameObject.SetActive(true);
        _concreteMixPanel.gameObject.SetActive(false);
    }

    private void Init()
    {
        _ingredientSelectionPanel.OnIngredientsAreCorrect += CheckIngredients;
        _concreteMixPanel.OnMixSucceeded += CheckConcreteMix;
    }

    private void Finish()
    {
        _ingredientSelectionPanel.OnIngredientsAreCorrect -= CheckIngredients;
        _concreteMixPanel.OnMixSucceeded -= CheckConcreteMix;
    }


    private void CheckIngredients(bool correct)
    {
        if (correct)
        {
            MissionManager.Instance.MissionCompleted();

            _ingredientSelectionPanel.gameObject.SetActive(false);
            _concreteMixPanel.gameObject.SetActive(true);
        }
        else
        {
            JobAreaManager.Instance.MinigameFailed();

            gameObject.SetActive(false);
        }
    }

    private void CheckConcreteMix(bool succeeded)
    {
        if (succeeded)
        {
            JobAreaManager.Instance.MinigameSuccessed();

            gameObject.SetActive(false);
        }
        else
        {
            JobAreaManager.Instance.MinigameFailed();

            gameObject.SetActive(false);
        }
    }
}
