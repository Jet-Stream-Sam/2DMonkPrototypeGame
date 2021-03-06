using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class UnlockedFeatures : MonoBehaviour
{
    [SerializeField] private List<Feature> features;
    private int unlockedFeaturesIndex = 0;
    public void InitiateUnlockSequence(Action onComplete)
    {
        if (features.Count == 0)
        {
            onComplete?.Invoke();
            return;
        }

        IterateFeatures(onComplete);
    }

    private void IterateFeatures(Action onComplete)
    {
        if(unlockedFeaturesIndex >= features.Count)
        {
            onComplete?.Invoke();
            return;
        }

        var feature = features[unlockedFeaturesIndex];
        feature.Show();

        if (unlockedFeaturesIndex < features.Count)
            feature.onFinish += () => IterateFeatures(onComplete);

        unlockedFeaturesIndex++;
    }
}

[Serializable]
public class Feature
{
    [SerializeField] private GameObject featureObj;
    [SerializeField] private PlayableDirector timeline;

    [HideInInspector] public Action onFinish;
    public void Show()
    {
        featureObj.SetActive(true);
        timeline.time = 0;
        timeline.Play();
        timeline.stopped += Hide;
    }

    public void Hide(PlayableDirector timeline)
    {
        timeline.stopped -= Hide;
        onFinish?.Invoke();
    }
}