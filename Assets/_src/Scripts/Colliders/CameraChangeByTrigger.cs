using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeByTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cameraToToggle;

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMainTrigger player = col.GetComponent<PlayerMainTrigger>();

        if (player == null)
            return;

        cameraToToggle.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        PlayerMainTrigger player = col.GetComponent<PlayerMainTrigger>();

        if (player == null)
            return;

        cameraToToggle.SetActive(false);
    }
}
