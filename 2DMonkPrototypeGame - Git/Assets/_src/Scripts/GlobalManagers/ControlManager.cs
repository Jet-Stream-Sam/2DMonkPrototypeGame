using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public InputMaster controls;
    public static ControlManager Instance { get; private set; }
    private void Awake()
    {
        #region Singleton
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }

        #endregion
        controls = new InputMaster();

    }

    private void OnEnable()
    {
        controls?.Enable();
    }

    private void OnDisable()
    {
        controls?.Disable();
    }
}
