using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveList : MonoBehaviour
{
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
    public PlayerMoves[] playerMoveList;

    public PlayerMoves Find(string name)
    {
        
        foreach(PlayerMoves move in playerMoveList)
        {
            if(name == move.animationClip.name)
            {
                return move;
            }
        }
        return null;
    }
}
