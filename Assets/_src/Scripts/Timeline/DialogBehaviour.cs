using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Playables;

public class DialogBehaviour : PlayableBehaviour
{
    private bool clipIsPlaying = false;
    private bool canPauseTimeline = false;
    private bool unpauseRequest = false;
    public DialogueObject dialogueObject;
    private PlayableDirector timeline;
    private MainDialogHandler dialogueHandler;
    private float waitSeconds;
    private double playablePreviousTime;
    private double playableDuration;

    public override void OnPlayableCreate(Playable playable)
    {
        timeline = playable.GetGraph().GetResolver() as PlayableDirector;

        Wait(0.1f);

    }

    public override void OnPlayableDestroy(Playable playable)
    {
        dialogueHandler.onDialogFinish -= SkipClip;
    }
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        playablePreviousTime = playable.GetPreviousTime();
        playableDuration = playable.GetDuration();

        if(dialogueHandler == null)
            dialogueHandler = playerData as MainDialogHandler;
        
        if (!dialogueHandler) return;

        if (clipIsPlaying)
            return;

        if (info.weight > 0)
        {
            clipIsPlaying = true;
            Debug.Log(dialogueHandler);
            dialogueHandler.dialogueObject = dialogueObject;
            dialogueHandler.InitiateDialog(timeline);
            dialogueHandler.onDialogFinish += SkipClip;
        }


    }
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
        if (canPauseTimeline)
        {
            if (Application.isPlaying)
            {
                if (!unpauseRequest)
                    dialogueHandler.PauseTimeline();
            }
            
            
            playablePreviousTime = 0;
            playableDuration = 0;
        }
        
    }


    private async void Wait(float seconds)
    {
        waitSeconds = seconds;

        while(waitSeconds > 0)
        {
            waitSeconds -= Time.deltaTime;
            await Task.Yield();
        }

        canPauseTimeline = true;
    }

    private void SkipClip()
    {
        bool atClipEnd = playableDuration == 0 && playablePreviousTime == 0;
        if (atClipEnd) return;
        unpauseRequest = true;
        timeline.time += playableDuration - playablePreviousTime;
        timeline.Evaluate();
        
    }
    
}
