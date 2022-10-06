using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerViewport : MonoBehaviour
{
    public float lookSpeed = 1f;

    private Vector2 mLook = Vector2.zero;

    public void OnLook(InputValue inputValue)
    {
        mLook = inputValue.Get<Vector2>();
    }

    public void Update()
    {
        Look();
    }

    private void Look()
    {
        var totalRotation = lookSpeed * Time.deltaTime * new Vector3(-mLook.y, mLook.x, 0f);
        transform.Rotate(Vector3.right, totalRotation.x);
        transform.Rotate(Vector3.up, totalRotation.y);
    }
}
