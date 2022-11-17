using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AlvinController : MonoBehaviour
{
    [System.Serializable]
    public struct ActionBinding
    {
        public KeyCode key;
        public UnityEvent action;
    }

    public ActionBinding[] alvinActionBindings;

    private void Update()
    {
        foreach (var binding in alvinActionBindings)
        {
            if (Input.GetKeyUp(binding.key))
                binding.action.Invoke();
        }
    }
}
