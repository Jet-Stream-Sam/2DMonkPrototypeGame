using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNCloseGates : MonoBehaviour
{
    [SerializeField] private GameObject gate;
    public void CloseGate()
    {
        gate.SetActive(true);
    }

    public void OpenGate()
    {
        gate.SetActive(false);
    }
}
