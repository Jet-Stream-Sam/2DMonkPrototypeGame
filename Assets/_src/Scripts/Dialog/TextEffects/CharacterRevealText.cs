using System.Collections;
using UnityEngine;
using TMPro;
using Sirenix.OdinInspector;
using System;
using System.Globalization;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class CharacterRevealText : MonoBehaviour
{
    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private LocalizeStringEvent localizeStringEvent;
    [SerializeField] [Range(0, 100)] private float revealBaseSpeed = 10f;
    private float revealResultSpeed => revealBaseSpeed * 0.5f;
    [SerializeField] private int characterFadeSpread = 10;
    private IEnumerator revealCharactersCoroutine;
    private float generalTimer;
    private float pauseTimer;
    
    private string currentDialogText;
    private int currentRawCharacter;
    private int currentDialogCharacter;
    private int initialCharacterIndex;
    private int characterCount;

    public Action onDialogResume;
    public Action onDialogStopped;


    public bool isWritingText { get; private set; } = false;

    private IEnumerator ValidateCharacters()
    {

        isWritingText = true;
        var textInfo = textComponent.textInfo;
        characterCount = textInfo.characterCount;

        pauseTimer = 0;

        Color32[] newColors;

        currentRawCharacter = currentDialogCharacter = initialCharacterIndex = 0;

        TagHandler.RefreshTextInfo(textComponent, ref characterCount, ref currentDialogText);

        bool inRange = true;
        
        while (inRange)
        {
            
            generalTimer = 0.1f;
            byte fadeSteps = (byte)Mathf.Max(1, 255 / characterFadeSpread);

            for (int i = initialCharacterIndex; i < currentDialogCharacter + 1; i++)
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
                    initialCharacterIndex++;

                    if(initialCharacterIndex == characterCount)
                    {
                        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                        currentDialogCharacter = 0;
                        currentRawCharacter = 0;
                        initialCharacterIndex = 0;
                        inRange = false;
                        isWritingText = false;
                        onDialogStopped?.Invoke();
                        yield break;
                    }
                }
            }

            textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

            bool hasTMPTag = TagHandler.CheckTMPTags(textComponent, textComponent.text[currentRawCharacter], ref currentRawCharacter);
            string customTag = TagHandler.CheckCustomTags(textComponent, ref characterCount, ref currentDialogText, currentDialogText[currentDialogCharacter], currentDialogCharacter, currentRawCharacter);
            bool hasCustomTag = !String.IsNullOrWhiteSpace(customTag);

            if (!hasCustomTag && pauseTimer <= 0)
            {
                currentDialogCharacter++;
                currentRawCharacter++;
            }
            else if(hasCustomTag)
            {
                ProcessTag(customTag);
            }
                
            if (currentDialogCharacter >= characterCount) currentDialogCharacter = characterCount - 1;
            if (currentRawCharacter >= textComponent.text.Length) currentRawCharacter = characterCount - 1;


            while (generalTimer > 0)
            {
                generalTimer -= Time.deltaTime * (revealBaseSpeed * 0.1f);
                if(pauseTimer > 0)
                {
                    pauseTimer -= Time.deltaTime;
                }
                yield return null;
            }

        }
        onDialogStopped?.Invoke();
    }

    private void RevealText()
    {

    }
    private void ProcessTag(string tagDescription)
    {
        string tagKey = "";
        string tagValue = "";
        if (tagDescription.IndexOf('=') != -1)
        {
            tagKey = tagDescription.Substring(0, tagDescription.IndexOf('='));
            tagValue = tagDescription.Substring(tagDescription.IndexOf('=') + 1);
        }

        if (!String.IsNullOrWhiteSpace(tagKey))
        {
            switch (tagKey)
            {
                case "pause":
                    pauseTimer = float.Parse(tagValue, CultureInfo.InvariantCulture.NumberFormat);
                    return;
                case "speed":
                    revealBaseSpeed = float.Parse(tagValue, CultureInfo.InvariantCulture.NumberFormat);
                    return;
                case "reveal_mode":
                    return;

            }
        }
    }
    public void WriteDialog(LocalizedString dialogue)
    {
        localizeStringEvent.StringReference.SetReference(dialogue.TableReference, dialogue.TableEntryReference);
        localizeStringEvent.StringReference.StringChanged += UpdateString;
    }

    private void UpdateString(string localizedText)
    {
        HideAllCharacters();
        textComponent.text = localizedText;
        StartCoroutine(revealCharactersCoroutine = ValidateCharacters());
        localizeStringEvent.StringReference.StringChanged -= UpdateString;
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
        TagHandler.EvaluateCharacters(textComponent, ref characterCount, ref currentDialogText);
        onDialogStopped?.Invoke();
        isWritingText = false;
        textComponent.alpha = 1;
        textComponent.ForceMeshUpdate();
    }
    
}


