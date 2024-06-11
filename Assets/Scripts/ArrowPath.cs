using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class ArrowPath : MonoBehaviour
    {
        [SerializeField] private List<Transform> _points;

        [SerializeField] private Player _player;

        private void OnEnable()
        {
            _player.ArrowSystem.SetPath(_points);
        }
    }
}
