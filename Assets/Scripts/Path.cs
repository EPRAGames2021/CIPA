using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField] private bool _correct;

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Material[] _materials;

    private void OnValidate()
    {
        if (_correct)
        {
            _meshRenderer.material = _materials[0];
        }
        else
        {
            _meshRenderer.material = _materials[1];
        }
    }

    private void Start()
    {
        if (_correct)
        {
            _meshRenderer.material = _materials[0];
        }
        else
        {
            _meshRenderer.material = _materials[1];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_correct && other.GetComponent<HealthSystem>() != null)
        {
            other.GetComponent<HealthSystem>().TakeDamage(2000);
        }
    }
}
