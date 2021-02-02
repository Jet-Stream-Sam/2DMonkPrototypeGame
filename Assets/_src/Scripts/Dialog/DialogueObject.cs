using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [Multiline(5)]
    public List<string> dialogueText;
}