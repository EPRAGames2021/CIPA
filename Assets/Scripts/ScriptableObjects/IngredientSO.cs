using UnityEngine;
using EPRA.Utilities;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Scriptable Objects/Ingredient", order = 1)]

public class IngredientSO : ResourceSO
{
    [SerializeField] private JobSector _jobSector;

    public JobSector JobSector => _jobSector;
}
