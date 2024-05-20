using EPRA.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientSelectionPanel : MonoBehaviour
{
    [SerializeField] private JobSector _jobSector;

    [SerializeField] private List<ResourceSO> _resourcesList;
    [SerializeField] private List<ResourceToggle> _resourcesToggleList;

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
        if (_resourcesToggleList.Count == 0) return;

        foreach (ResourceToggle resourceToggle in _resourcesToggleList)
        {
            resourceToggle.ResetToggle();
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

        foreach (ResourceToggle resourceToggle in _resourcesToggleList)
        {
            IngredientSO ingredientSO = resourceToggle.ResourceSO as IngredientSO;

            if (ingredientSO.JobSector == _jobSector && !resourceToggle.IsSelected ||
                ingredientSO.JobSector != _jobSector && resourceToggle.IsSelected)
            {
                canProcceed = false;
            }
        }

        OnIngredientsAreCorrect?.Invoke(canProcceed);
    }


}
