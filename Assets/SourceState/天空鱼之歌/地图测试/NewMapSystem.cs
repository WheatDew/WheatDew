using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapSystem : MonoBehaviour
{
    private static NewMapSystem _instance;
    public static NewMapSystem instance { get { return _instance; } }

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public MapElement[] memeber;
    
}
