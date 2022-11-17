using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[RequireComponent(typeof(Camera), typeof(RotateRender))]
public class VehicleCamera : MonoBehaviour
{
    public string outputFolder;
    public TogglingAttributes offStateAttributes;
    [HideInInspector]
    public TogglingAttributes onStateAttributes;

    private Camera _camera;
    private RotateRender _rotateRender;
    private bool _active;
    public bool Active
    {
        get { return _active; }
        set { _active = value; SetCameraAttributes(); }
    }


    private void Start()
    {
        _camera = GetComponent<Camera>();
        _rotateRender = GetComponent<RotateRender>();
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

    private void SaveRenderToFile(RenderTexture renderTexture)
    {
        Rect targetTextureRect = new Rect(0, 0, renderTexture.width, renderTexture.height);

        Texture2D photo = new Texture2D(
            (int)targetTextureRect.width, (int)targetTextureRect.height,
            TextureFormat.RGBA32, false);
        photo.ReadPixels(targetTextureRect, 0, 0);

        string dateTimeString = System.DateTime.Now.ToString("yyyy-MM-dd_T_HH-mm-ss");
        string outputFileName = $"{dateTimeString}_{gameObject.name}.png";
        string outputFilePath = Path.Combine(outputFolder, outputFileName);

        Debug.Log($"Saving photo at: {outputFilePath}");
        File.WriteAllBytes(outputFilePath, photo.EncodeToPNG());

        DestroyImmediate(photo);
    }

    public void TakePhoto()
    {
        var oldActiveTexture = RenderTexture.active;
        RenderTexture.active = _camera.targetTexture;
        _camera.Render();

        RenderTexture outputRT = new RenderTexture(_camera.targetTexture)
        {
            width = _camera.targetTexture.height,
            height = _camera.targetTexture.width
        };
        _rotateRender.ApplyRotation(_camera.targetTexture, outputRT, flip: true);

        SaveRenderToFile(outputRT);

        RenderTexture.active = oldActiveTexture;
        DestroyImmediate(outputRT);        
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
