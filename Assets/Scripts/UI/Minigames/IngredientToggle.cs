using UnityEngine;
using UnityEngine.UI;
using TMPro;
using EPRA.Utilities;

public class IngredientToggle : MonoBehaviour
{
    [SerializeField] private Toggle _toggle;

    [SerializeField] private IngredientSO _ingredientSO;

    [SerializeField] private TextMeshProUGUI _ingredientName;
    [SerializeField] private Image _ingredientIcon;


    public IngredientSO IngredientSO => _ingredientSO;
    public bool IsSelected => _toggle.isOn;

    private void OnEnable()
    {
        Refresh();
    }


    private void Refresh()
    {
        if (_ingredientSO != null)
        {
            SetIngredient(_ingredientSO);
        }
    }


    public void SetIngredient(IngredientSO ingredientSO)
    {
        _ingredientSO = ingredientSO;

        _ingredientName.text = LanguageManager.GetTranslation(_ingredientSO.NameKey);
        _ingredientIcon.sprite = _ingredientSO.Icon;
    }

    public void ResetToggle()
    {
        _toggle.isOn = false;
    }
}
