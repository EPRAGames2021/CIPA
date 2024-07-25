using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class PaintingRoom : MonoBehaviour
    {
        [SerializeField] private GameObject _baseWalls;
        [SerializeField] private GameObject _paintedWalls;

        private void Start()
        {
            SetRoomPainted(false);
        }


        public void TransformRoom()
        {
            SetRoomPainted(true);
        }

        private void SetRoomPainted(bool painted)
        {
            _baseWalls.SetActive(!painted);
            _paintedWalls.SetActive(painted);
        }
    }
}
