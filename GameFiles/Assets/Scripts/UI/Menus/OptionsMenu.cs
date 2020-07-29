using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public void BackMenu(GameObject activatedMenu)
    {
        activatedMenu.SetActive(true);

        MenuFirstSelected menuScript = activatedMenu.GetComponent<MenuFirstSelected>();
        menuScript.ChangeFirstButtonSelected();
        gameObject.SetActive(false);

    }
}
