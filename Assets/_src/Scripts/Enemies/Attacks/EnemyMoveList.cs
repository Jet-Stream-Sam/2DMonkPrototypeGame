using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveList : MonoBehaviour
{
    [ListDrawerSettings(ShowIndexLabels = true, ListElementLabelName = "name")]
    public EnemyMoves[] enemyMoveList;

    public EnemyMoves Find(string name)
    {

        foreach (EnemyMoves move in enemyMoveList)
        {
            if (name == move.animationClip.name)
            {
                return move;
            }
        }
        return null;
    }
}