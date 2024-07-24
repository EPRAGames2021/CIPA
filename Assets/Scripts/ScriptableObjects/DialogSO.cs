using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace CIPA
{
    [CreateAssetMenu(fileName = "Dialog", menuName = "Scriptable Objects/Dialog", order = 1)]
    public class DialogSO : ScriptableObject
    {
        [SerializeField] private string _speaker;
        [SerializeField] private Sprite _speakerIcon;

        [SerializeField] private List<string> _dialogs;

        public string Speaker => _speaker;
        public Sprite SpeakerIcon => _speakerIcon;
        public List<string> Dialogs => _dialogs;
    }
}
