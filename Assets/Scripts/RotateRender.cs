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
        ApplyRotation(source, destination);
    }

    public void ApplyRotation(RenderTexture source, RenderTexture destination, bool flip = false)
    {
        rotateRenderMaterial.SetVector("_ViewportRect", mViewportVector);   // set every time to support edit mode too

        Graphics.Blit(source, destination, rotateRenderMaterial);
        if (flip)
        {
            // 180-degree flipping necessary to save renders to file but not in material, for some reason
            Graphics.Blit(destination, source, rotateRenderMaterial);
            Graphics.Blit(source, destination, rotateRenderMaterial);
        }
    }
}
