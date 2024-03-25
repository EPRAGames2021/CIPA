using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EPRA.Utilities;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient", order = 1)]

public class IngredientSO : MonoBehaviour
{
    [SerializeField] private string _name;
    [SerializeField] private string _nameKey;

    [SerializeField] private Sprite _icon;

    public string Name => _name;
    public string NameKey => _nameKey;
    public Sprite Icon => _icon;
}
