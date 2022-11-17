using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticedRange : MonoBehaviour
{
    public CCharacter character;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"&&!character.isDeath)
        {
            character.noticed = other.gameObject;
            //character.agent.isStopped = false;
        }
    }
}
