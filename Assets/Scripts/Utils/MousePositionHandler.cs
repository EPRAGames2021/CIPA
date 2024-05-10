using UnityEngine;

[RequireComponent(typeof(MouseDelta))]
public class MousePositionHandler : MonoBehaviour
{
    private MouseDelta _mouseDelta;

    [SerializeField] private RectTransform _areaToHandle;

    [SerializeField] private float _heightPercent;
    [SerializeField] private float _widthPercent;

    [SerializeField] private Vector2 _innerBoxSize;
    [SerializeField] private Vector2 _innerBoxPosition;
    [SerializeField] private Vector2 _touchPositionRelativeToInnerBox;

    public float HeightPercent => _heightPercent;
    public float WidthPercent => _widthPercent;

    public Vector2 InnerBoxSize => _innerBoxSize;
    public Vector2 InnerBoxPosition => _innerBoxPosition;
    public Vector2 TouchPosition => _touchPositionRelativeToInnerBox;

    private void OnValidate()
    {
        if (_mouseDelta == null) _mouseDelta = GetComponent<MouseDelta>();

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
        //_touchPositionRelativeToInnerBox = new Vector2(_mouseDelta.LastMousePosition.x, _mouseDelta.LastMousePosition.y) - _innerBoxPosition;
        _touchPositionRelativeToInnerBox = _mouseDelta.LastMousePosition - _innerBoxSize;

        _widthPercent = ((_touchPositionRelativeToInnerBox.x / _innerBoxSize.x) * 100) + 50;
        _heightPercent = ((_touchPositionRelativeToInnerBox.y / _innerBoxSize.y) * 100) + 50;

        _widthPercent = Mathf.Clamp(_widthPercent, 0, 100);
        _heightPercent = Mathf.Clamp(_heightPercent, 0, 100);
    }
}
