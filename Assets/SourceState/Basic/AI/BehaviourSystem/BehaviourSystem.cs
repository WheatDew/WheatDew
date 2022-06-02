using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSystem : MonoBehaviour
{
    private static BehaviourSystem _s;
    public static BehaviourSystem s { get { return _s; } }

    private void Awake()
    {
        if (_s != null) _s = this;
    }

    public Dictionary<string, Behaviour> behaviours = new Dictionary<string, Behaviour>();

}
