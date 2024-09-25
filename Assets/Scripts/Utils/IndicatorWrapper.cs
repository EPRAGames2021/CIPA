using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class IndicatorWrapper : MonoBehaviour
    {
        [SerializeField] private GameObject _gameObject;
        [SerializeField] private Sprite _icon;

        public GameObject GameObject => _gameObject;
        public Sprite Icon => _icon;
    }
}
