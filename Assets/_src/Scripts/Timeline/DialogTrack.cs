using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackColor(220f/255f, 22f/255f, 60f/255f)]
[TrackBindingType(typeof(MainDialogHandler))]
[TrackClipType(typeof(DialogClip))]
public class DialogTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<Light2DMixer>.Create(graph, inputCount);
    }
}
