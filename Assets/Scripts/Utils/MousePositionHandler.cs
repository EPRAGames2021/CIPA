using UnityEngine;

[RequireComponent(typeof(MouseDelta))]
public class MousePositionHandler : MonoBehaviour
{
    [SerializeField] private MouseDelta _mouseDelta;

    [SerializeField] private RectTransform _areaToHandle;

    [SerializeField] private float _heightPercent;
    [SerializeField] private float _widthPercent;

    [SerializeField] private Vector2 _innerBoxSize;
    [SerializeField] private Vector2 _innerBoxPosition;
    [SerializeField] private Vector2 _touchPositionRelativeToInnerBox;

    public float HeightPercent => _heightPercent;
    public float WidthPercent => _widthPercent;

    private void OnValidate()
    {
        _innerBoxSize = _areaToHandle.rect.size;
        _innerBoxPosition = _areaToHandle.position;
    }

    private void Start()
    {
        _innerBoxSize = _areaToHandle.rect.size;
        _innerBoxPosition = _areaToHandle.position;
    }

    private void Update()
    {
        _touchPositionRelativeToInnerBox = _mouseDelta.LastMousePosition - _innerBoxSize;

        _widthPercent = ((_touchPositionRelativeToInnerBox.x / _innerBoxSize.x) * 100) + 50;
        _heightPercent = ((_touchPositionRelativeToInnerBox.y / _innerBoxSize.y) * 100) + 50;

        _widthPercent = Mathf.Clamp(_widthPercent, 0, 100);
        _heightPercent = Mathf.Clamp(_heightPercent, 0, 100);
    }
}
