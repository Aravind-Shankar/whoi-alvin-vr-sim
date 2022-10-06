using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    private PlayerControllerStandard mPCStandard;
    private PlayerControllerPilot mPCPilot;
    private PlayerControllerViewport mPCViewport;

    private PlayerInput mPlayerInput;

    private Vector3 mOldLocalPosition = Vector3.zero;

    private void Start()
    {
        mPCStandard = GetComponent<PlayerControllerStandard>();
        mPCPilot = GetComponent<PlayerControllerPilot>();
        mPCViewport = GetComponent<PlayerControllerViewport>();

        mPlayerInput = GetComponent<PlayerInput>();
        SwitchController(mPlayerInput.defaultActionMap);
    }

    public void OnInteract(InputValue inputValue)
    {
        bool hit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo);
        if (hit)
        {
            Interact(hitInfo.collider.gameObject);
        }
    }

    public void OnExit(InputValue inputValue)
    {
        SwitchController(mPlayerInput.defaultActionMap);
        transform.localPosition = mOldLocalPosition;
    }
    
    private void SwitchController(string actionMapName)
    {
        mPlayerInput.SwitchCurrentActionMap(actionMapName);

        mPCStandard.enabled = false;
        mPCPilot.enabled = false;
        mPCViewport.enabled = false;
        switch (actionMapName)
        {
            case "standard":
                mPCStandard.enabled = true;
                break;
            case "pilot":
                mPCPilot.enabled = true;
                break;
            case "viewport":
                mPCViewport.enabled = true;
                break;
        }
    }

    private void Interact(GameObject otherObject)
    {
        if (otherObject.CompareTag("PilotControls"))
        {
            SwitchController("pilot");
            mOldLocalPosition = transform.localPosition;
            transform.position = Vector3.Lerp(transform.position, otherObject.transform.position, 0.7f);
        }
        else if (otherObject.CompareTag("Viewport"))
        {
            SwitchController("viewport");
            mOldLocalPosition = transform.localPosition;
            transform.position = Vector3.Lerp(transform.position, otherObject.transform.position, 0.95f);
        }
    }
}
