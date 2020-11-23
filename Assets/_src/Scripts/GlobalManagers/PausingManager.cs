﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PausingManager : MonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;
    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;
        SceneManager.activeSceneChanged += SceneChanged;

    }

    
    public static bool isGamePaused = false;
  
    public void Pause(GameObject pMenu)
    {
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

    private void SceneChanged(Scene arg0, Scene arg1)
    {
        controls.Player.Enable();
        isGamePaused = false;
        Time.timeScale = 1;
    }
}
