using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EPRA.Utilities;

public class PPESelectionMenu : MenuController
{
    [SerializeField] private List<ResourceToggle> _resourceToggleList;

    [SerializeField] private Button _confirmButton;

    [Header("Debug")]
    [SerializeField] private JobSO _jobSO;

    [SerializeField] private List<EquipmentType> _requiredEquipments;
    [SerializeField] private List<EquipmentType> _selectedEquipments;


    public static event System.Action<bool> OnSelectionIsCorrect;


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
        _jobSO = JobAreaManager.Instance?.JobSectorAreaSO.CurrentJob;
        _requiredEquipments = _jobSO.RequiredEquipment;
        _selectedEquipments = new();

        foreach (ResourceToggle resourceToggle in _resourceToggleList)
        {
            resourceToggle.ResetToggle();
        }

        _confirmButton.onClick.AddListener(CheckCorrectPPE);
    }

    private void Finish()
    {
        _jobSO = null;
        _requiredEquipments = null;
        _selectedEquipments.Clear();

        _confirmButton.onClick.RemoveAllListeners();
    }


    private void CheckCorrectPPE()
    {
        foreach (ResourceToggle resourceToggle in _resourceToggleList)
        {
            if (resourceToggle.IsSelected)
            {
                EquipmentSO equipment = resourceToggle.ResourceSO as EquipmentSO;

                _selectedEquipments.Add(equipment.Type);
            }
        }

        bool sameSet = ListExtensions.HaveSameElements(_requiredEquipments, _selectedEquipments);

        OnSelectionIsCorrect?.Invoke(sameSet);
    }


    public override void SelectUI()
    {
        _resourceToggleList[0].Toggle.Select();
    }
}
