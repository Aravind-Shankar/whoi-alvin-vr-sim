using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VehicleCamera : MonoBehaviour
{
    [System.Serializable]
    public struct TogglingAttributes
    {
        public CameraClearFlags clearFlags;
        public Color backgroundColor;
        public LayerMask cullingMask;

        public void ReadFromCamera(Camera camera)
        {
            clearFlags = camera.clearFlags;
            backgroundColor = camera.backgroundColor;
            cullingMask = camera.cullingMask;
        }

        public void WriteToCamera(Camera camera)
        {
            camera.clearFlags = clearFlags;
            camera.backgroundColor = backgroundColor;
            camera.cullingMask = cullingMask;
        }
    }
    public TogglingAttributes offStateAttributes;
    [HideInInspector]
    public TogglingAttributes onStateAttributes;

    private Camera _camera;
    private bool _active;
    public bool Active
    {
        get { return _active; }
        set { _active = value; SetCameraAttributes(); }
    }


    private void Start()
    {
        _camera = GetComponent<Camera>();
        onStateAttributes.ReadFromCamera(_camera);
        Active = true;
    }

    private void SetCameraAttributes()
    {
        (_active ? onStateAttributes : offStateAttributes).WriteToCamera(_camera);
    }

    public void Toggle()
    {
        Active = !Active;
    }
}
