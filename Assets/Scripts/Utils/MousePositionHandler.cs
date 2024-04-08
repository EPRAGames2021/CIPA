using UnityEngine;

[RequireComponent(typeof(MouseDelta))]
public class MousePositionHandler : MonoBehaviour
{
    private MouseDelta _mouseDelta;

    [SerializeField] private RectTransform _areaToHandle;

    [SerializeField] private float _heightPercent;
    [SerializeField] private float _widthPercent;

    public float HeightPercent => _heightPercent;
    public float WidthPercent => _widthPercent;

    private void OnValidate()
    {
        if (_mouseDelta == null) _mouseDelta = GetComponent<MouseDelta>();
    }

    private void LateUpdate()
    {
        Vector2 innerBoxSize = _areaToHandle.rect.size;
        Vector2 innerBoxPosition = _areaToHandle.position;

        Vector2 touchPositionRelativeToInnerBox = new Vector2(_mouseDelta.LastMousePosition.x, _mouseDelta.LastMousePosition.y) - innerBoxPosition;

        _widthPercent = (touchPositionRelativeToInnerBox.x / innerBoxSize.x * 100) + 50;
        _heightPercent = (touchPositionRelativeToInnerBox.y / innerBoxSize.y * 100) + 50;

        _widthPercent = Mathf.Clamp(_widthPercent, 0, 100);
        _heightPercent = Mathf.Clamp(_heightPercent, 0, 100);
    }
}
