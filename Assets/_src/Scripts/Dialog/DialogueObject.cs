using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;


[CreateAssetMenu(menuName = "Scriptable Objects/DialogueObject")]


public class DialogueObject : ScriptableObject
{
    public List<Dialogue> dialogues;
}

[Serializable]
public class Dialogue
{
    public string characterName;
    public Sprite characterPortrait;
    public bool isMysteriousCharacter = false;
    public bool isDialogueUnskippable = false;
    public bool dialogueBlocksAdvancing = false;
    
    [InfoBox("Please try to break lines in order to not make the text overflow. There's no automatic wrapping on the text.")]
    [InfoBox("The tags for this dialogue only accept TMP tags and custom defined tags. Any invalid tags" +
        " are treated as TMP tags and may cause unintended bugs.", InfoMessageType.Warning)]
    public List<LocalizedString> localizedDialogueText;

}