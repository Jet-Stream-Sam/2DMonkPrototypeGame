using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Animation Events/Instantiate Object")]
public class InstantiateObject : AnimationEventSO
{
    public GameObject instObject;
    public bool usesLocalVFXTransform;
}
