using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 mMove = Vector2.zero;
    private float mMoveVertical = 0f;

    public void OnMove(InputAction.CallbackContext context)
    {
        mMove = context.ReadValue<Vector2>();
    }

    public void OnMoveVertical(InputAction.CallbackContext context)
    {
        mMoveVertical = context.ReadValue<float>();
    }

    public void Update()
    {
        Move();
    }

    private void Move()
    {
        Vector3 totalMovement = new Vector3(mMove.x, mMoveVertical, mMove.y);
        if (totalMovement.sqrMagnitude < 0.01)
            return;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        //var move = Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(totalMovement.x, 0, totalMovement.y);
        var move = totalMovement;
        transform.position += move * scaledMoveSpeed;
    }
}
