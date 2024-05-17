using EPRA.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSelectionPanel : MonoBehaviour
{
    [SerializeField] private JobSector _jobSector;

    [SerializeField] private List<IngredientSO> _ingredientsList;

    [SerializeField] private List<IngredientToggle> _ingredientsToggleList;
    [SerializeField] private GameObject _ingredientTogglePrefab;

    [SerializeField] private GameObject _ingredientsContainer;

    [SerializeField] private Button _confirmButton;

    public event System.Action<bool> OnIngredientsAreCorrect;


    private void OnEnable()
    {
        ResetToggles();
    }

    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }

    private void ResetToggles()
    {
        if (_ingredientsToggleList.Count == 0) return;

        foreach (IngredientToggle ingredientToggle in _ingredientsToggleList)
        {
            ingredientToggle.ResetToggle();
        }
    }

    private void Init()
    {
        _confirmButton.onClick.AddListener(CheckIngredients);
    }

    private void Finish()
    {
        _confirmButton.onClick.RemoveAllListeners();
    }


    private void CheckIngredients()
    {
        bool canProcceed = true;

        foreach (IngredientToggle ingredientToggle in _ingredientsToggleList)
        {
            if (ingredientToggle.IngredientSO.JobSector == _jobSector && !ingredientToggle.IsSelected ||
                ingredientToggle.IngredientSO.JobSector != _jobSector && ingredientToggle.IsSelected)
            {
                canProcceed = false;
            }
        }

        OnIngredientsAreCorrect?.Invoke(canProcceed);
    }


}
