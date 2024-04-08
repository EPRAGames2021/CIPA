using UnityEngine;

public class ConcreteBarrel : MonoBehaviour
{
    [SerializeField] private ScreenTouchController _screenTouchController;
    [SerializeField] private ConcreteMixPanel _concreteMixPanel;
    [SerializeField] private GameObject _concreteBarrel;

    private void Update()
    {
        Spin();
    }


    private void Spin()
    {
        if (_concreteMixPanel.MixFinished) return;

        if (_screenTouchController.DetectHolding())
        {
            _concreteBarrel.transform.Rotate(0, 0, _concreteMixPanel.AverageSpeed * 1.5f);
        }
    }
}
