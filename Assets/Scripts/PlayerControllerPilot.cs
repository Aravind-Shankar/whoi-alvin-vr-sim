using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerPilot : PlayerControllerBase
{
    public float alvinMoveSpeed = 1f;
    public float lookSpeed = 1f;

    private Vector2 mAlvinMove = Vector2.zero;
    private float mAlvinMoveVertical = 0f;
    private Vector2 mLook = Vector2.zero;

    public void OnMove(InputValue inputValue)
    {
        mAlvinMove = inputValue.Get<Vector2>();
    }

    public void OnMoveVertical(InputValue inputValue)
    {
        mAlvinMoveVertical = inputValue.Get<float>();
    }

    public void OnLook(InputValue inputValue)
    {
        mLook = inputValue.Get<Vector2>();
    }

    public void Update()
    {
        if (alvinController == null)
            return;

        MoveAlvin();
        Look();
    }

    private void MoveAlvin()
    {
        Vector3 totalMovement = new Vector3(mAlvinMove.x, mAlvinMoveVertical, mAlvinMove.y);
        if (totalMovement.sqrMagnitude < 0.01)
            return;
        //totalMovement = Quaternion.Euler(0, alvinObject.transform.eulerAngles.y, 0) * totalMovement;
        var scaledMoveSpeed = alvinMoveSpeed * Time.deltaTime;
        alvinController.transform.position += totalMovement * scaledMoveSpeed;
    }

    private void Look()
    {
        var totalRotation = lookSpeed * Time.deltaTime * new Vector3(-mLook.y, mLook.x, 0f);
        transform.Rotate(Vector3.right, totalRotation.x);
        transform.Rotate(Vector3.up, totalRotation.y);
    }
}
