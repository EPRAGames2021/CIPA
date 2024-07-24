using UnityEngine;

[System.Serializable]
public class TrackableAction
{
    [SerializeField] private string _action;
    [SerializeField] private bool _performed;

    public string Action { get { return _action; } set { _action = value; } }
    public bool Performed { get { return _performed; } set { _performed = value; } }


    public TrackableAction(string action, bool performed)
    {
        _action = action;
        _performed = performed;
    }
}