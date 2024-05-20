using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

public class ResourceToggle : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    [SerializeField] private ResourceSO _resourceSO;

    [SerializeField] private TextMeshProUGUI _ingredientName;
    [SerializeField] private Image _ingredientIcon;


    public ResourceSO ResourceSO => _resourceSO;
    public bool IsSelected => _toggle.isOn;

    private void OnEnable()
    {
        Refresh();
    }


    private void Refresh()
    {
        if (_resourceSO != null)
        {
            SetEquipment(_resourceSO);
        }
    }


    public virtual void SetEquipment(ResourceSO resourceSO)
    {
        _resourceSO = resourceSO;

        _ingredientName.text = LanguageManager.GetTranslation(_resourceSO.NameKey);
        _ingredientIcon.sprite = _resourceSO.Icon;
    }

    public void ResetToggle()
    {
        _toggle.isOn = false;
    }
}
