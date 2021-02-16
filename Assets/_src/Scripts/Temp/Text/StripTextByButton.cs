using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class StripTextByButton : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private int stripStartIndex;
    [SerializeField] private int stripCount;
    private Keyboard keyboard;
    private void Start()
    {
        keyboard = InputSystem.GetDevice<Keyboard>();
    }
    private void Update()
    {
        if (keyboard.kKey.wasPressedThisFrame)
        {
            StripText(textComponent.text, stripStartIndex, stripCount);
        }
    }
    private void StripText(string text, int startIndex, int count)
    {
        textComponent.text = text.Remove(startIndex, count);
        ShowCharactersAt(startIndex);
    }

    private void ShowCharactersAt(int index)
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;
        for (int i = 0; i < index; i++)
        {
            var characterInfo = textInfo.characterInfo[i];
            if (!characterInfo.isVisible) continue;

            int materialIndex = characterInfo.materialReferenceIndex;

            var newColors = textInfo.meshInfo[materialIndex].colors32;

            int vertexIndex = characterInfo.vertexIndex;

            for (int j = 0; j < 4; j++)
            {
                newColors[vertexIndex + j].a = 255;
                newColors[vertexIndex + j].b = 20;
            }
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        
    }
}
