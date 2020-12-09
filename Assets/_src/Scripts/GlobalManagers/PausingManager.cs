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
    
    public void Pause(GameObject pMenu)
    {
        if (!canPause)
            return;
        if (pMenu.activeInHierarchy || !isGamePaused)
        {
            isGamePaused = !isGamePaused;

            pMenu.SetActive(isGamePaused);
            Time.timeScale = isGamePaused ? 0 : 1;

            if (isGamePaused)
            {
                pMenu.GetComponent<MenuFirstSelected>().ChangeFirstButtonSelected();
                controls.Player.Disable();

            }
            else
            {
                controls.Player.Enable();
            }

            
}
        
    }

    public void PauseReset()
    {
        isGamePaused = false;
        Time.timeScale = 1;
        controls.Player.Enable();
    }

    private void SceneChanged(Scene arg0, Scene arg1)
    {
        controls.Player.Enable();
        isGamePaused = false;
        Time.timeScale = 1;
    }
}
