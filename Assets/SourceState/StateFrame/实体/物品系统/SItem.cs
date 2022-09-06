using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SItem 
{
    private static Dictionary<string,CPack> packs=new Dictionary<string, CPack> ();

    public static void AddPack(string key,CPack pack)
    {
        packs.Add(key, pack);
    }
    public static void AddPack(CPack pack)
    {
        packs.Add(pack.transform.GetInstanceID().ToString(), pack);
    }
    public static void RemovePack(string key)
    {
        packs.Remove(key);
    }
    public static void DestroyPack(string key)
    {
        Object.DestroyImmediate(packs[key]);
        packs.Remove(key);
    }
    public static void DestroyPackGameObject(string key)
    {
        Object.DestroyImmediate(packs[key].gameObject);
        packs.Remove(key);
    }

    public static int ItemCountGain(string packKey,string itemKey,int count)
    {
        return 0;
    }
}
