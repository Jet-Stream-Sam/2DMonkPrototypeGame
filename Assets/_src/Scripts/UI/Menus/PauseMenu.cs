using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private LoadingManager loadingManager;
    private ControlManager controlManager;
    private InputMaster controls;
    [SerializeField] private string currentLevelName = "Main Level";
    [SerializeField] private string levelToLoadName = "Main Menu Ex";
    [SerializeField] private GameObject showCanvasObj;

    private void Start()
    {
        controlManager = ControlManager.Instance;
        loadingManager = LoadingManager.Instance;
        controls = controlManager.controls;
        controls.UIExtra.Pause.performed += Escape;
    }
    public void ResumeGame()
    {
        PausingManager pManager = PausingManager.Instance;
        pManager.Pause(showCanvasObj);
    }

    public void OptionsMenu(GameObject activatedMenu)
    {
        activatedMenu.SetActive(true);

        MenuFirstSelected menuScript = activatedMenu.GetComponent<MenuFirstSelected>();
        menuScript.ChangeFirstButtonSelected();
        showCanvasObj.SetActive(false);

    }

    public void BackToMainMenu()
    {
        loadingManager.InitiateLoad(currentLevelName, levelToLoadName);
        
    }

    private void Escape(InputAction.CallbackContext callbackContext)
    {
        PausingManager pManager = PausingManager.Instance;
        
        pManager.Pause(showCanvasObj);
    }

    private void OnDestroy()
    {
        controls.UIExtra.Pause.performed -= Escape;
    }
}
