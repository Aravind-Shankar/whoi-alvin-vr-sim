using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerControllerBase))]
public class ControllerManager : MonoBehaviour
{
    private Dictionary<string, PlayerControllerBase> mPlayerControllers = new Dictionary<string, PlayerControllerBase>();
    private PlayerInput mPlayerInput;
    private Vector3 mOldLocalPosition = Vector3.zero;

    private void Start()
    {
        foreach (var controller in this.GetComponents<PlayerControllerBase>())
        {
            mPlayerControllers.Add(controller.inputActionMapName, controller);
            controller.enabled = false;
        }

        mPlayerInput = GetComponent<PlayerInput>();
        if (!mPlayerControllers.ContainsKey(mPlayerInput.defaultActionMap))
            throw new MissingComponentException($"No controller for default map {mPlayerInput.defaultActionMap}!");

        foreach (var map in mPlayerInput.actions.actionMaps)
        {
            if (!mPlayerControllers.ContainsKey(map.name))
                Debug.LogWarning($"No corresponding controller found for action map: {map.name}. Ignore if unnecessary.");
        }
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
    
    private bool SwitchController(string actionMapName)
    {
        if (!mPlayerControllers.ContainsKey(actionMapName))
            return false;

        mPlayerInput.SwitchCurrentActionMap(actionMapName);
        foreach (var entry in mPlayerControllers)
        {
            entry.Value.enabled = (entry.Key == actionMapName);
        }

        return true;
    }

    private void Interact(GameObject otherObject)
    {
        if (otherObject.CompareTag("PilotControls") && SwitchController("pilot"))
        {
            mOldLocalPosition = transform.localPosition;
            transform.position = Vector3.Lerp(transform.position, otherObject.transform.position, 0.7f);
        }
        else if (otherObject.CompareTag("Viewport") && SwitchController("viewport"))
        {
            mOldLocalPosition = transform.localPosition;
            transform.position = Vector3.Lerp(transform.position, otherObject.transform.position, 0.95f);
        }
    }
}
