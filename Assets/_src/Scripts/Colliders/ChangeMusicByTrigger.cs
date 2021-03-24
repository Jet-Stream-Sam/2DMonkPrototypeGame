using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicByTrigger : MonoBehaviour
{
    [SerializeField] private SetMusic music;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.TryGetComponent(out PlayerMainTrigger player))
            return;

        music.PlayMusic();
    }

}
