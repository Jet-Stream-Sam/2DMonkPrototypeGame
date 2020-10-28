using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackMoveList : MonoBehaviour
{
    public PlayerAttack[] playerAttackMoveList;

    public PlayerAttack Find(string name)
    {
        foreach(PlayerAttack attack in playerAttackMoveList)
        {
            if(name == attack.animationName)
            {
                return attack;
            }
        }
        return null;
    }
}
