using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SRange
{
    //���
    public static Dictionary<string, CRange> cranges = new Dictionary<string, CRange>();
    

    public static void Add(string key,CRange cRange)
    {
        cranges.Add(key, cRange);
    }


}



