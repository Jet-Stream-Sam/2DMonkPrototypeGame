using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;
    [SerializeField] private string levelName = "Main Menu Ex";
    [SerializeField] private GameObject showCanvasObj;

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;
        controls.UI.Cancel.performed += Escape;
    }
    public void ResumeGame()
    {
        PausingManager pManager = controlManager.GetComponent<PausingManager>();
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
        SceneManager.LoadScene(levelName);
    }

    private void Escape(InputAction.CallbackContext callbackContext)
    {
        PausingManager pManager = controlManager.GetComponent<PausingManager>();
        pManager.Pause(showCanvasObj);
    }

    private void OnDestroy()
    {
        controls.UI.Cancel.performed -= Escape;
    }
}
