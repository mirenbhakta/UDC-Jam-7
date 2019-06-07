using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class UIInput : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onEscapeKey;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)) return;

        onEscapeKey.Invoke();
    }
}
