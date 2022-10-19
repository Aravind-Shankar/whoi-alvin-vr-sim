using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, ImageEffectAllowedInSceneView]
[RequireComponent(typeof(Camera))]
public class RotateRender : MonoBehaviour
{
    public Material rotateRenderMaterial;

    private Camera mCamera;
    private Vector4 mViewportVector;

    private void Awake()
    {
        mCamera = GetComponent<Camera>();
        mViewportVector = new Vector4(mCamera.rect.x, mCamera.rect.y, mCamera.rect.width, mCamera.rect.height);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        rotateRenderMaterial.SetVector("_ViewportRect", mViewportVector);   // set every time to support edit mode too

        Graphics.Blit(source, destination, rotateRenderMaterial);
    }
}
