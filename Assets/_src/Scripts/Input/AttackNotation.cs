using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackNotation
{
    
    public string allowedState;
    [HideIf("freeMovementAttack")]
    public float allowedTimeBetweenInputs = 0.2f;

    [OnValueChanged("NotationCheck")]
    [BoxGroup("Notation")]
    [TabGroup("Notation/Movement Input", "Movement Input")]
    public PlayerInputHandler.MovementInputNotation[] movementNotation;
    [TabGroup("Notation/Movement Input", "Button Input")]
    public PlayerInputHandler.ButtonInputNotation buttonNotation;
    private bool freeMovementAttack;

    public AttackNotation(PlayerInputHandler.MovementInputNotation[] movementNotation, 
                          PlayerInputHandler.ButtonInputNotation buttonNotation)
    {
        this.movementNotation = movementNotation;
        this.buttonNotation = buttonNotation;
    }

    void NotationCheck()
    {
        if(movementNotation.Length > 0)
        {
            freeMovementAttack = false;
        }
        else
        {
            freeMovementAttack = true;
        }
    }
}
