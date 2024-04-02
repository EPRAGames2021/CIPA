using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Pipe : MonoBehaviour
{
    [SerializeField] private PipeType _type;

    [SerializeField] private int _rotation;
    [SerializeField] private bool _attached;


    public bool Attached { get { return _attached; } set { _attached = value; } }


    private void Start()
    {
        Init();
    }

    private void OnDestroy()
    {
        Finish();
    }

    private void OnMouseDown()
    {
        Debug.Log("clicked");
    }


    private void Init()
    {
        
    }

    private void Finish()
    {

    }


    public void Rotate()
    {
        _rotation = (_rotation + 90) % 360;

        transform.eulerAngles = new(transform.rotation.x, transform.rotation.y, transform.rotation.z + _rotation);
    }
}

public enum PipeType
{
    Starting = 0,
    Ending = 1,
    Straight = 2,
    LShapedLeft = 3,
    LShapedRight = 4,
}