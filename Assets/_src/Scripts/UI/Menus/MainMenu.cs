using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private LoadingManager loadingManager;
    [SerializeField] private string currentLevelName = "Main Menu Ex";
    [SerializeField] private string levelToLoadName = "Main Level";

    private void Start()
    {
        loadingManager = LoadingManager.Instance;
    }
    public void PlayGame()
    {
        loadingManager.InitiateLoad(currentLevelName, levelToLoadName);
    }

    public void OptionsMenu(GameObject activatedMenu)
    {
        activatedMenu.SetActive(true);

        MenuFirstSelected menuScript = activatedMenu.GetComponent<MenuFirstSelected>();
        menuScript.ChangeFirstButtonSelected();
        gameObject.SetActive(false);

    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
