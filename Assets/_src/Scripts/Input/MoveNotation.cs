using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MoveNotation
{

    [OnValueChanged("NotationCheck")]
    [BoxGroup("Notation")]
    [TabGroup("Notation/Movement Input", "Movement Input")]
    public PlayerInputHandler.MovementInputNotation[] movementNotation;
    [TabGroup("Notation/Movement Input", "Button Input")]
    public PlayerInputHandler.ButtonInputNotation buttonNotation;
    private bool freeMovementMove = true;

    [PropertyOrder(1)]
    public string allowedState;
    [PropertyOrder(1)] [HideIf("freeMovementMove")]
    public float allowedTimeBetweenInputs = 0.2f;

    [HideInInspector] public bool needsToBeHeld = false;

    public MoveNotation(PlayerInputHandler.MovementInputNotation[] movementNotation, 
                          PlayerInputHandler.ButtonInputNotation buttonNotation)
    {
        this.movementNotation = movementNotation;
        this.buttonNotation = buttonNotation;
    }

    
    public void NotationCheck()
    {
        if(movementNotation.Length > 0)
        {
            freeMovementMove = false;
        }
        else
        {
            freeMovementMove = true;
        }
    }

    [HideIf("needsToBeHeld")]
    [Button("Pressed To Execute", ButtonSizes.Medium)]
    [GUIColor(0.7f, 1f, 0.2f)]
    void ChangeButton()
    {
        needsToBeHeld = true;
    }
    [ShowIf("needsToBeHeld")]
    [Button("Pressed and Held", ButtonSizes.Medium)]
    [GUIColor(0.7f, 0.2f, 1f)]
    void ChangeButton2()
    {
        needsToBeHeld = false;
    }
}
