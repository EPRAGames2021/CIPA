using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class FaceCamera : MonoBehaviour
    {
        private void LateUpdate()
        {
            transform.LookAt(Camera.main.transform.position);
        }
    }
}
