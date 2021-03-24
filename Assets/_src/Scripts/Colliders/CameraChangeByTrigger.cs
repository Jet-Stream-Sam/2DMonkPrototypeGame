using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraChangeByTrigger : MonoBehaviour
{
    [SerializeField] private GameObject cameraToToggle;

    private void OnTriggerEnter2D(Collider2D col)
    {

        if (!col.TryGetComponent(out PlayerMainTrigger player))
            return;

        cameraToToggle.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.TryGetComponent(out PlayerMainTrigger player))
            return;

        cameraToToggle.SetActive(false);
    }
}
