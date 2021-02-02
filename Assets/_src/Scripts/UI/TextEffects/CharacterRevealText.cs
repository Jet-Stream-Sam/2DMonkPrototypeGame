using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;

public class CharacterRevealText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] [Range(0, 100)] private float revealSpeed = 10f;
    [SerializeField] private int characterFadeSpread = 10;
    private IEnumerator revealCharactersCoroutine;
    private float timer;

    public Action onDialogResume;
    public Action onDialogStopped;

    public bool isWritingText { get; private set; } = false;
    private void Start()
    {
        HideAllCharacters();
    }
    private IEnumerator RevealCharacters()
    {
        
        textComponent.alpha = 0;
        textComponent.ForceMeshUpdate();

        isWritingText = true;
        var textInfo = textComponent.textInfo;
        int characterCount = textInfo.characterCount;
        Color32[] newColors;

        int initialCharacterIndex = 0;
        int currentCharacter = initialCharacterIndex;
        
        bool inRange = true;
        
        while (inRange)
        {
            timer = 0.1f;
            byte fadeSteps = (byte)Mathf.Max(1, 255 / characterFadeSpread);

            for (int i = initialCharacterIndex; i < currentCharacter + 1; i++)
            {
                var characterInfo = textInfo.characterInfo[i];
                if (!characterInfo.isVisible) continue;

                int materialIndex = characterInfo.materialReferenceIndex;

                newColors = textInfo.meshInfo[materialIndex].colors32;

                int vertexIndex = characterInfo.vertexIndex;

                byte alpha = (byte)Mathf.Clamp(newColors[vertexIndex].a + fadeSteps, 0, 255);

                for (int j = 0; j < 4; j++)
                {
                    newColors[vertexIndex + j].a = alpha;
                }

                if (alpha == 255)
                {
                    initialCharacterIndex += 1;

                    if(initialCharacterIndex == characterCount)
                    {
                        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
                        
                        currentCharacter = 0;
                        initialCharacterIndex = 0;
                        inRange = false;
                        isWritingText = false;
                    }
                }
            }

            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            currentCharacter += 1;
            if (currentCharacter >= characterCount) currentCharacter = characterCount - 1;

            while(timer > 0)
            {
                timer -= Time.deltaTime * (revealSpeed * 0.1f);
                yield return null;
            }

            
        }
        onDialogStopped?.Invoke();
    }
    
    public void WriteDialog(string dialogue)
    {
        textComponent.text = dialogue;
        HideAllCharacters();
        
        StartCoroutine(revealCharactersCoroutine = RevealCharacters());
    }
    [Button("Hide All Characters")]
    public void HideAllCharacters()
    {
        if(revealCharactersCoroutine != null)
            StopCoroutine(revealCharactersCoroutine);
        onDialogResume?.Invoke();
        isWritingText = false;
        textComponent.alpha = 0;
        textComponent.ForceMeshUpdate();
        
    }
    [Button("Show All Characters")]
    public void ShowAllCharacters()
    {
        if (revealCharactersCoroutine != null)
            StopCoroutine(revealCharactersCoroutine);
        onDialogStopped?.Invoke();
        isWritingText = false;
        textComponent.alpha = 1;
        textComponent.ForceMeshUpdate();
    }
    
}
