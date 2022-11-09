using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRange : MonoBehaviour
{
    public CharacterMovement character;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"&&!character.isDeath)
        {
            character.noticed = other.gameObject;
            //character.agent.isStopped = false;
        }
    }
}
