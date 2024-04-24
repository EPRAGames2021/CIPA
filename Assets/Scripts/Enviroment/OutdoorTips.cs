using UnityEngine;
using EPRA.Utilities;

public class OutdoorTips : MonoBehaviour
{
    [SerializeField] private SimpleTranslate _translatePanel;

    [SerializeField] private int _tipsAmount;

    private void OnEnable()
    {
        Translate();
    }


    private void Translate()
    {
        int tip = Random.Range(0, _tipsAmount);

        string key = "outdoorTip" + tip;

        _translatePanel.SetKey(key);
    }
}
