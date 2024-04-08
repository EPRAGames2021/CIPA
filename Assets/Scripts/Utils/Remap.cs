using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Remap
{
    public static float RemapValue(float value, float oldMin, float oldMax, float newMin, float newMax)
    {
        value = Mathf.Clamp(value, oldMin, oldMax);

        float mappedValue = newMin + (newMax - newMin) * ((value - oldMin) / (oldMax - oldMin));

        return mappedValue;
    }
}
