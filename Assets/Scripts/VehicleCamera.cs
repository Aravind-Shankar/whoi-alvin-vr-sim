using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class VehicleCamera : MonoBehaviour
{
    public string outputFolder;
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

    public void TakePhoto()
    {
        var oldActiveTexture = RenderTexture.active;
        RenderTexture.active = _camera.targetTexture;
        _camera.Render();

        Rect targetTextureRect = new Rect(0, 0, _camera.targetTexture.width, _camera.targetTexture.height);

        Texture2D photo = new Texture2D(
            (int) targetTextureRect.width, (int) targetTextureRect.height,
            TextureFormat.RGB24, false);
        photo.ReadPixels(targetTextureRect, 0, 0);

        string dateTimeString = System.DateTime.Now.ToString("yyyy-MM-dd_T_HH-mm-ss");
        string outputFileName = $"{dateTimeString}_{gameObject.name}.png";
        string outputFilePath = Path.Combine(outputFolder, outputFileName);
        Debug.Log($"Saving photo at: {outputFilePath}");
        File.WriteAllBytes(outputFilePath, photo.EncodeToPNG());

        RenderTexture.active = oldActiveTexture;
    }


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

}
