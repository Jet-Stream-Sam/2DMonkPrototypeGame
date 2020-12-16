using UnityEngine;
using System.Collections;
using Sirenix.OdinInspector;

public class AnimationEffectsManager : MonoBehaviour
{
    [SerializeField] private Transform placeSpot;
    [SerializeField] private MainVFXManager mainVFXManager;

    public void RecieveEffect(InstantiateObject obj)
    {
        Instantiate(obj.instObject, placeSpot.position, Quaternion.identity, mainVFXManager.transform);
    }
}
