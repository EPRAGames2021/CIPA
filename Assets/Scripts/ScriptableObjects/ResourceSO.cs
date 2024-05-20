using UnityEngine;
using EPRA.Utilities;

[CreateAssetMenu(fileName = "Resource", menuName = "Scriptable Objects/Resource", order = 1)]

public class ResourceSO : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private string _nameKey;

    [SerializeField] private Sprite _icon;

    public string Name => _name;
    public string NameKey => _nameKey;
    public Sprite Icon => _icon;


    private void OnValidate()
    {
        _name = name;
    }
}
