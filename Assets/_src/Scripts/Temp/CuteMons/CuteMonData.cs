using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Scriptable Objects/Test/CuteMonData")]
public class CuteMonData : ScriptableObject
{

    public string monName;
    public string naturalHabitat;
    public string primaryType;
    public string secondaryType;
    [SerializeField]
    private int attack, defense, speed;
    
}
