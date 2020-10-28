using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string levelName = "Main Level";
    public void PlayGame()
    {
        SceneManager.LoadScene(levelName);
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
