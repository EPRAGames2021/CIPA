using System.Collections.Generic;
using UnityEngine;

public class PlayerVehicle : MonoBehaviour
{
    [SerializeField] private bool _carrying;

    [SerializeField] private List<GameObject> _trunkContent;

    public bool Carrying => _carrying;


    private void OnValidate()
    {
        SetCarrying(_carrying);
    }

    private void OnEnable()
    {
        SetCarrying(_carrying);
    }


    public void SetCarrying(bool carrying)
    {
        _carrying = carrying;

        for (int i = 0; i < _trunkContent.Count; i++)
        {
            _trunkContent[i].SetActive(_carrying);
        }
    }
}
