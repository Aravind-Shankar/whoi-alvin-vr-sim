using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlvinController : MonoBehaviour
{
    [System.Serializable]
    public struct ComponentToggle<T>
    {
        public T component;
        public KeyCode toggleKey;
    }

    public ComponentToggle<Light>[] lightToggles;
    public ComponentToggle<VehicleCamera>[] vehicleCameraToggles;

    private void Update()
    {
        foreach (var lightToggle in lightToggles)
        {
            if (Input.GetKeyUp(lightToggle.toggleKey))
                ToggleLight(lightToggle.component);
        }

        foreach (var cameraToggle in vehicleCameraToggles)
        {
            if (Input.GetKeyUp(cameraToggle.toggleKey))
                cameraToggle.component.Toggle();
        }
    }

    private void ToggleLight(Light light)
    {
        light.enabled = !light.enabled;
    }
}
