using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class CuteMonChoose : MonoBehaviour
{
    Keyboard kb;
    [SerializeField] private CuteMonData storedCuteMon;
    [SerializeField] private Vector2 projectedLevels;
    private void Start()
    {
        kb = InputSystem.GetDevice<Keyboard>();
    }
    private void Update()
    {
        if (kb.eKey.wasPressedThisFrame)
        {
            CuteMon selectedCuteMon = 
                new CuteMon(storedCuteMon, 
                "", 
                (int)Random.Range(projectedLevels.x, projectedLevels.y));

            if(selectedCuteMon.Nickname == selectedCuteMon.CData.monName) print($"Congrats! " +
                $"You've acquired a {selectedCuteMon.CData.monName}!");
            else print($"Congrats! You've acquired a {selectedCuteMon.CData.monName}, " +
                $"with a nickname of {selectedCuteMon.Nickname}!");

            MethodDelay.DelayMethodByTimeASync(
                () => print($"It's level is {selectedCuteMon.Level}!"), 0.5f);
            MethodDelay.DelayMethodByTimeASync(
                () => print($"Psst. It's IVs are {selectedCuteMon.InternalValue}!"), 1f);
            
        }
    }
}
