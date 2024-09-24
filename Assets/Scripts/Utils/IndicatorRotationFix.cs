using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CIPA
{
    public class IndicatorRotationFix : MonoBehaviour
    {
        private void Update()
        {
            transform.eulerAngles = new Vector3(0f, 0f, transform.eulerAngles.z);
        }
    }
}
