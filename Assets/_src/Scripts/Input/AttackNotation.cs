using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackNotation
{
    public string allowedState;
    public float allowedTimeBetweenInputs = 0.2f;
    public PlayerInputHandler.MovementInputNotation[] movementNotation;
    public PlayerInputHandler.ButtonInputNotation buttonNotation;

    public AttackNotation(PlayerInputHandler.MovementInputNotation[] movementNotation, 
                          PlayerInputHandler.ButtonInputNotation buttonNotation)
    {
        this.movementNotation = movementNotation;
        this.buttonNotation = buttonNotation;
    }
}
