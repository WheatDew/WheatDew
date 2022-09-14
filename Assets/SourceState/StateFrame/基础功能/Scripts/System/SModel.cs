using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SModel
{
    public static Dictionary<string, GameObject> models = new Dictionary<string, GameObject>();

    public static void Add(string name, GameObject model)
    {
        models.Add(name, model);
    }
}
