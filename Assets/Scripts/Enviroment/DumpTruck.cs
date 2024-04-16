using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpTruck : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private ScreenTouchController _controller;

    private void OnValidate()
    {
        if (_animator == null)
        {
            _animator = GetComponent<Animator>();
        }
    }

    private void Update()
    {
        if (_controller == null)
        {
            //Debug.Log("Controller is null");

            return;
        }

        Dump(_controller.DetectHolding());
    }

    public void Dump(bool dump)
    {
        _animator.SetBool("IsDumping", dump);
    }
}
