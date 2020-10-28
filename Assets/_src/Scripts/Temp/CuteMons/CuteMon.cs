using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuteMon
{
    public CuteMonData CData { get; private set; }
    public string Nickname { get; private set; }
    public int Level { get; private set; }
    public int InternalValue { get; private set; }

    public CuteMon(CuteMonData cData, string nickname, int level)
    {
        CData = cData;
        if (nickname != "") Nickname = nickname;
        else Nickname = CData.monName;
        Level = level;
        InternalValue = Random.Range(0, 16);
    }
}
