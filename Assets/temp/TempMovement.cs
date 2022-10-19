using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempMovement : MonoBehaviour
{
    public float force;

    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.localPosition += force * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        rb.AddForce(force * transform.forward);
    }
}
