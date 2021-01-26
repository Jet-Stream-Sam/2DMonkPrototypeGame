using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StartCutsceneByTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMainTrigger player = col.GetComponent<PlayerMainTrigger>();
        if (player == null)
            return;
        timeline.Play();
    }

    
}
