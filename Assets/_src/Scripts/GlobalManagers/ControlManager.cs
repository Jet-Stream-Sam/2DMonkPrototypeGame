using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public InputMaster controls;
    public static ControlManager Instance { get; private set; }

    private List<GameObject> playerControlEmitters = new List<GameObject>();
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

    public void EnablePlayerControls(GameObject emitter)
    {
        ClearNullObjects();
        playerControlEmitters.Remove(emitter);
        if (playerControlEmitters.Count == 0)
            controls.Player.Enable();
    }
    public void DisablePlayerControls(GameObject emitter)
    {
        ClearNullObjects();
        playerControlEmitters.Add(emitter);

        controls.Player.Disable();
    }

    private void ClearNullObjects()
    {
        for(int i = playerControlEmitters.Count - 1; i >= 0; i--)
        {
            if(playerControlEmitters[i] == null)
            {
                playerControlEmitters.RemoveAt(i);
            }
        }
    }

    public void ClearObjects()
    {
        playerControlEmitters.Clear();
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
