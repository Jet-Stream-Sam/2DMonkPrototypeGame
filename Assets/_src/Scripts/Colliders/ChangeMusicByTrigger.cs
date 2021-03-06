using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMusicByTrigger : MonoBehaviour
{
    [SerializeField] private SetMusic music;

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMainTrigger player = col.GetComponent<PlayerMainTrigger>();

        if (player == null)
            return;

        music.PlayMusic();
    }

}
