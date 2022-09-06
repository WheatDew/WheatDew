using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPack : MonoBehaviour
{
    public string key;
    public Dictionary<string, CPack> packs = new Dictionary<string, CPack>();

    private void Start()
    {
        key = transform.GetInstanceID().ToString();
        SItem.AddPack(key, this);
    }
}

public class PackData
{
    public string name;
    public int count;
}
