using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerStandard : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lookSpeed = 1f;

    private Vector2 mMove = Vector2.zero;
    private Vector2 mLook = Vector2.zero;

    public void OnMove(InputValue inputValue)
    {
        mMove = inputValue.Get<Vector2>();
    }

    public void OnLook(InputValue inputValue)
    {
        mLook = inputValue.Get<Vector2>();
    }

    public void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector3 totalMovement = new Vector3(mMove.x, 0f, mMove.y);
        if (totalMovement.sqrMagnitude < 0.01)
            return;
        totalMovement = Quaternion.Euler(0, transform.eulerAngles.y, 0) * totalMovement;
        var scaledMoveSpeed = moveSpeed * Time.deltaTime;
        transform.position += totalMovement * scaledMoveSpeed;
    }

    private void Look()
    {
        var totalRotation = lookSpeed * Time.deltaTime * new Vector3(-mLook.y, mLook.x, 0f);
        transform.Rotate(Vector3.right, totalRotation.x);
        transform.Rotate(Vector3.up, totalRotation.y);
    }
}
