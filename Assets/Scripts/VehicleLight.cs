using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class VehicleLight : MonoBehaviour
{
    private Light _light;

    void Start()
    {
        _light = GetComponent<Light>();
    }

    public void Toggle()
    {
        _light.enabled = !_light.enabled;
    }
}
