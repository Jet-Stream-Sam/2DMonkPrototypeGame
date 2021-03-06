using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausingManager : MonoBehaviour
{
    public static PausingManager Instance { get; private set; }
    private ControlManager controlManager;
    private InputMaster controls;

    private List<GameObject> pauseControlEmitters = new List<GameObject>();
    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;
        SceneManager.activeSceneChanged += SceneChanged;

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
    }


    public static bool canPause = false;
    public static bool isGamePaused = false;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Init()
    {
        canPause = true;
        isGamePaused = false;
    }
    public void Pause(GameObject pMenu)
    {
        if (!canPause)
            return;
        if (pMenu.activeInHierarchy || !isGamePaused)
        {
            isGamePaused = !isGamePaused;

            pMenu.SetActive(isGamePaused);

            if (isGamePaused)
            {
                Time.timeScale = 0;
                pMenu.GetComponent<MenuFirstSelected>().ChangeFirstButtonSelected();
                controlManager.DisablePlayerControls(gameObject);

            }
            else
            {
                if (pauseControlEmitters.Count == 0)
                    Time.timeScale = 1;
                
                controlManager.EnablePlayerControls(gameObject);
            }

            
}
        
    }

    public void PauseUnblock(GameObject emitter)
    {
        ClearNullObjects();
        pauseControlEmitters.Remove(emitter);
    }
    public void PauseBlock(GameObject emitter)
    {
        ClearNullObjects();
        pauseControlEmitters.Add(emitter);

    }

    public void PauseReset()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        controls.Player.Enable();
    }

    private void ClearNullObjects()
    {
        for (int i = pauseControlEmitters.Count - 1; i >= 0; i--)
        {
            if (pauseControlEmitters[i] == null)
            {
                pauseControlEmitters.RemoveAt(i);
            }
        }
    }
    public void ClearObjects()
    {
        pauseControlEmitters.Clear();
    }
    private void SceneChanged(Scene arg0, Scene arg1)
    {
        controlManager.EnablePlayerControls(gameObject);
        isGamePaused = false;
        Time.timeScale = 1;
    }
}
