using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorSystem : MonoBehaviour
{
    private static MapEditorSystem _instance;
    public static MapEditorSystem Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
    }
}
