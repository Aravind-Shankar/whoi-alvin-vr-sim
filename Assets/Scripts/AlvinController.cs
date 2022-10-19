using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class AlvinController : MonoBehaviour
{
    private Rigidbody mRigidbody;

    private void Awake()
    {
        mRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        AddHoveringForce();
    }

    private void AddHoveringForce()
    {
        mRigidbody.AddForce(-mRigidbody.mass * Physics.gravity);
    }
}
