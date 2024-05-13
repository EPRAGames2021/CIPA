using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteFormHandler : MonoBehaviour
{
    [SerializeField] private ConcreteForm _concreteForm;

    [SerializeField] private FoundationAndStructureUI _foundation;
    [SerializeField] private ScreenTouchController _controller;

    private void Update()
    {
        _concreteForm.SetActive(_controller.DetectHolding() && _foundation.StageIndex == 3);
    }
}
