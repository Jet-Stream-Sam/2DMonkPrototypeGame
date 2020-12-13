
using System;
using UnityEngine;

public interface IEntityController
{
    Action<ScriptableObject> AnimationEventWasCalled { get; set; }
}
