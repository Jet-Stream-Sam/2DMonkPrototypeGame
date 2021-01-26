using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class EnemyDeathCutscene : MonoBehaviour
{
    [SerializeField] private PlayableDirector timeline;
    [SerializeField] private EnemyMainController enemyController;

    private void Start()
    {
        enemyController.hasDied += TriggerCutscene;
    }

    private void TriggerCutscene()
    {
        timeline.Play();
    }
}
