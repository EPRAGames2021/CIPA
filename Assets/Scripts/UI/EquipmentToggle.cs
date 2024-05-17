using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

public class EquipmentToggle : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    [SerializeField] private EquipmentSO _equipmentSO;

    [SerializeField] private TextMeshProUGUI _ingredientName;
    [SerializeField] private Image _ingredientIcon;


    public EquipmentSO EquipmentSO => _equipmentSO;
    public bool IsSelected => _toggle.isOn;

    private void OnEnable()
    {
        Refresh();
    }


    private void Refresh()
    {
        if (_equipmentSO != null)
        {
            SetEquipment(_equipmentSO);
        }
    }


    public void SetEquipment(EquipmentSO equipmentSO)
    {
        _equipmentSO = equipmentSO;

        _ingredientName.text = LanguageManager.GetTranslation(_equipmentSO.NameKey);
        _ingredientIcon.sprite = _equipmentSO.Icon;
    }

    public void ResetToggle()
    {
        _toggle.isOn = false;
    }
}
