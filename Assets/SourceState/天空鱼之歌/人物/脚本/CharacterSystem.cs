using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterSystem : MonoBehaviour
{
    
}

public class Behaviour
{
    public UnityAction behaviour;
    public float probability;

    public Behaviour(UnityAction behaviour)
    {
        this.behaviour = behaviour;
        this.probability = 1;
    }

    public Behaviour(float probability,UnityAction behaviour)
    {
        this.behaviour = behaviour;
        this.probability = probability;
    }
}
