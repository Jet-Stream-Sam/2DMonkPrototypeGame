
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuFirstSelected : MonoBehaviour
{
    public GameObject firstSelectedButton;

    public void ChangeFirstButtonSelected()
    {
        EventSystem.current.SetSelectedGameObject(null);

        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}
