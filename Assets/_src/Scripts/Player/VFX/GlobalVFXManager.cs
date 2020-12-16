using UnityEngine;
using System.Collections;

public class GlobalVFXManager : MonoBehaviour
{
    public static GlobalVFXManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


    }
}
