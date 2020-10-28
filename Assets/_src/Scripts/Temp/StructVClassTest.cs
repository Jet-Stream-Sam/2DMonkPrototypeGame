using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructVClassTest : MonoBehaviour
{
    public struct StructStats
    {
        public int strength;
        public int intelligence;
        public int etc;
    }

    public class ClassStats
    {
        public int strength;
        public int intelligence;
        public int etc;
    }

    private void Start()
    {
        print("---STRUCT STATS---");
        StructStats s_stats = new StructStats();
        s_stats.strength = 15;

        print(s_stats.strength);
        s_stats.strength = 20;
        print(s_stats.strength);

        StructStats otherS_Stats = s_stats;
        s_stats.strength = 25;
        print(otherS_Stats.strength);

        print("---CLASS STATS---");
        ClassStats c_stats = new ClassStats();
        c_stats.strength = 15;

        print(c_stats.strength);
        c_stats.strength = 20;
        print(c_stats.strength);

        ClassStats otherC_Stats = c_stats;
        c_stats.strength = 25;
        print(otherC_Stats.strength);


    }
}
