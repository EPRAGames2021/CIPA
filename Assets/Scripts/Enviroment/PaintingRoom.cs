using UnityEngine;

public class PaintingRoom : MonoBehaviour
{
    [SerializeField] private MousePositionHandler _mousePositionHandler;

    [SerializeField] private MeshRenderer _paintRenderer;

    [SerializeField] private FinishingUI _finishingUI;

    [SerializeField] private GameObject _inkRoll;
    private float _inkRollX;
    private float _inkRollZ;
    [SerializeField] private float _inkRollMinHeight;
    [SerializeField] private float _inkRollMaxHeight;

    private void OnEnable()
    {
        _inkRollX = _inkRoll.transform.position.x;
        _inkRollZ = _inkRoll.transform.position.z;

        Color color = new(_paintRenderer.material.color.r, _paintRenderer.material.color.g, _paintRenderer.material.color.b, 0.0f);

        _paintRenderer.material.color = color;
    }

    private void Update()
    {
        UpdateColor();
        MoveInkRoll();
    }


    private void UpdateColor()
    {
        if (_finishingUI.PaintFinished) return;

        float alpha = Remap.RemapValue(_finishingUI.CompletionPercentage, 0, 100, 0, 1);

        Color newColor = new(_paintRenderer.material.color.r, _paintRenderer.material.color.g, _paintRenderer.material.color.b, alpha);

        _paintRenderer.material.color = newColor;
    }

    private void MoveInkRoll()
    {
        if (_finishingUI.PaintFinished) return;

        float height = Remap.RemapValue(_mousePositionHandler.HeightPercent, 0, 100, _inkRollMinHeight, _inkRollMaxHeight);

        _inkRoll.transform.position = new Vector3(_inkRollX, height, _inkRollZ);
    }
}
