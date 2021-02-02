using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;

public class MainDialogHandler : MonoBehaviour
{
    private ControlManager controlManager;
    private InputMaster controls;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private PlayableDirector currentTimeline;
    [FoldoutGroup("Dependencies")]
    public DialogueObject dialogueObject;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private CharacterRevealText textAdvancer;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Image imagePortrait;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private Image imagePortraitShadow;
    [FoldoutGroup("Dependencies")]
    [SerializeField] private TMP_Text characterNameTitle;
    [SerializeField] private bool playOnStart = true;
    private bool isDialogActive = false;

    private int dialogueCurrentIndex = 0;
    private int dialogueTextCurrentIndex = 0;

    public Action onDialogInitiate;
    public Action onDialogResume;
    public Action onDialogStopped;
    public Action onDialogFinish;

    private void Start()
    {
        controlManager = ControlManager.Instance;
        controls = controlManager.controls;

        CleanWindow();

        controls.Dialog.Advance.performed += ProcessDialog;
        onDialogFinish += ResumeTimeline;
        textAdvancer.onDialogResume += DialogResume;
        textAdvancer.onDialogStopped += DialogStop;
        

        if (playOnStart)
            InitiateDialog(currentTimeline);
    }
    private void CleanWindow()
    {
        textAdvancer.HideAllCharacters();
        characterNameTitle.text = "";
        imagePortrait.sprite = null;
        imagePortrait.color = Color.clear;
        imagePortraitShadow.color = Color.clear;
    }
    public void ResumeTimeline()
    {
        CleanWindow();
        currentTimeline.playableGraph.GetRootPlayable(0).SetSpeed(1);
    }

    public void PauseTimeline()
    {
        currentTimeline.playableGraph.GetRootPlayable(0).SetSpeed(0);
    }

    public void InitiateDialog(PlayableDirector timeline)
    {
        
        if (!dialogueObject) return;

        dialogueCurrentIndex = 0;
        dialogueTextCurrentIndex = 0;
        currentTimeline = timeline;

        ApplyDialogInfo();

        onDialogInitiate?.Invoke();
        isDialogActive = true;
    }

    public void ProcessDialog(InputAction.CallbackContext ctx)
    {
        if (PausingManager.isGamePaused) return;
        if (!isDialogActive) return;
        

        List<string> dialogueSequences = dialogueObject.dialogues[dialogueCurrentIndex].dialogueText;

        if (textAdvancer.isWritingText && !dialogueObject.dialogues[dialogueCurrentIndex].isDialogueUnskippable)
        {
            textAdvancer.ShowAllCharacters();
            onDialogStopped?.Invoke();
            return;
        }

        if (!textAdvancer.isWritingText && !dialogueObject.dialogues[dialogueCurrentIndex].dialogueBlocksAdvancing)
        {

            dialogueTextCurrentIndex += 1;

            if (dialogueTextCurrentIndex >= dialogueSequences.Count)
            {
                dialogueCurrentIndex += 1;
                dialogueTextCurrentIndex = 0;
            }

            if (dialogueCurrentIndex >= dialogueObject.dialogues.Count)
            {
                EndDialog();
                return;
            }

            ApplyDialogInfo();
            return;
        }

    }

    private void ApplyDialogInfo()
    {
        imagePortrait.sprite = dialogueObject.dialogues[dialogueCurrentIndex].characterPortrait;
        imagePortrait.color = Color.white;
        imagePortraitShadow.color = new Color(0, 0, 0, 0.3f);
        if (dialogueObject.dialogues[dialogueCurrentIndex].isMysteriousCharacter)
            characterNameTitle.text = "???";
        else
            characterNameTitle.text = dialogueObject.dialogues[dialogueCurrentIndex].characterName;

        List<string> dialogueSequences = dialogueObject.dialogues[dialogueCurrentIndex].dialogueText;
        textAdvancer.WriteDialog(dialogueSequences[dialogueTextCurrentIndex]);

    }

    public void DialogResume()
    {
        onDialogResume?.Invoke();
    }
    public void DialogStop()
    {
        if (dialogueObject != null)
            if (dialogueObject.dialogues[dialogueCurrentIndex].dialogueBlocksAdvancing)
                return;
        onDialogStopped?.Invoke();
    }
    public void EndDialog()
    {
        dialogueCurrentIndex = 0;
        dialogueTextCurrentIndex = 0;
        isDialogActive = false;
        textAdvancer.HideAllCharacters();
        onDialogFinish?.Invoke();
    }
    private void OnDestroy()
    {
        controls.Dialog.Advance.performed -= ProcessDialog;
    }
}
