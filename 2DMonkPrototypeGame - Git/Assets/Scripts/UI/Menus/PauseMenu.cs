using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameManager globalManager;
    private InputMaster controls;
    public string levelName = "Main Menu Ex";
    public GameObject showCanvasObj;

    private void Start()
    {
        globalManager = GameManager.Instance;
        controls = globalManager.controls;
        controls.UI.Cancel.performed += Escape;
    }
    public void ResumeGame()
    {
        PausingManager pManager = globalManager.GetComponent<PausingManager>();
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
        PausingManager pManager = globalManager.GetComponent<PausingManager>();
        pManager.Pause(showCanvasObj);
    }

    private void OnDestroy()
    {
        controls.UI.Cancel.performed -= Escape;
    }
}
