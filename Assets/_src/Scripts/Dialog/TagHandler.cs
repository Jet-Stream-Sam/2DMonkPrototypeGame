using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class TagHandler
{
    public static bool CheckTMPTags(TMP_Text textComponent, char character, ref int rawIndex)
    {
        if (character == '<')
        {
            int tagIndex = rawIndex;
            string tagDescription = "";
            while (tagIndex < textComponent.text.Length)
            {
                tagIndex++;
                char tagCharacter = textComponent.text[tagIndex];
                if (tagCharacter == '>')
                {
                    break;
                }
                tagDescription += tagCharacter;

            }

            bool customTag = ValidateTag(tagDescription, false);

            if (!customTag)
            {
                rawIndex += tagDescription.Length + 2;
                return true;
            }

        }
        return false;
    }
    public static string CheckCustomTags(TMP_Text textComponent, ref int characterCount, ref string formattedText, char character, int formattedIndex, int rawIndex)
    {
        if (character == '<')
        {
            int tagIndex = formattedIndex;
            string tagDescription = "";
            while (tagIndex < formattedText.Length)
            {
                tagIndex++;
                char tagCharacter = formattedText[tagIndex];
                if (tagCharacter == '>')
                {
                    break;
                }
                tagDescription += tagCharacter;

            }

            bool customTag = ValidateTag(tagDescription, true);

            if (customTag)
            {
                StripText(textComponent, rawIndex, tagDescription.Length + 2);
                RefreshTextInfo(textComponent, ref characterCount, ref formattedText);
                ShowCharactersAt(textComponent, formattedIndex);
                return tagDescription;
            }

        }
        return null;
    }
    public static void StripText(TMP_Text textComponent, int startIndex, int count)
    {
        textComponent.text = textComponent.text.Remove(startIndex, count);

    }
    public static void RefreshTextInfo(TMP_Text textComponent, ref int characterCount, ref string formattedText)
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;
        characterCount = textInfo.characterCount;
        formattedText = "";
        for (int i = 0; i < characterCount; i++)
        {
            var characterInfo = textInfo.characterInfo[i];
            formattedText += characterInfo.character;
        }
    }
    public static void ShowCharactersAt(TMP_Text textComponent, int index)
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
                //newColors[vertexIndex + j].b = 20;
            }
        }
        textComponent.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

    }
    public static void EvaluateCharacters(TMP_Text textComponent, ref int characterCount, ref string formattedText)
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;
        characterCount = textInfo.characterCount;
        RefreshTextInfo(textComponent, ref characterCount, ref formattedText);

        int evaluateFormattedIndex = 0;
        int evaluateRawIndex = 0;

        while (evaluateFormattedIndex < characterCount)
        {
            bool hasTMPTag = CheckTMPTags(textComponent, textComponent.text[evaluateRawIndex], ref evaluateRawIndex);
            string customTag = CheckCustomTags(textComponent, ref characterCount, ref formattedText, formattedText[evaluateFormattedIndex], evaluateFormattedIndex, evaluateRawIndex);
            bool hasCustomTag = !String.IsNullOrWhiteSpace(customTag);

            if (!hasCustomTag)
            {
                evaluateFormattedIndex++;
                evaluateRawIndex++;
            }

        }
    }
    public static bool ValidateTag(string tagDescription, bool applyTagChanges)
    {
        string tagKey = "";
        string tagValue = "";
        if (tagDescription.IndexOf('=') != -1)
        {
            tagKey = tagDescription.Substring(0, tagDescription.IndexOf('='));
            tagValue = tagDescription.Substring(tagDescription.IndexOf('=') + 1);
        }

        if(!String.IsNullOrWhiteSpace(tagKey))
        {
            switch (tagKey)
            {
                case "pause":
                    if (!applyTagChanges)
                        return true;
                    return true;
                case "speed":
                    if (!applyTagChanges)
                        return true;
                    return true;
                case "reveal_mode":
                    if (!applyTagChanges)
                        return true;
                    return true;
            }
        }
        return false;
    }
}