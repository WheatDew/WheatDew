using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticedRange : MonoBehaviour
{
    public CCharacter character;

    private string target_tag = "";

    private GameObject currentTarget;

    private float timer = 0;
    public float calculateInterval=5;


    private void Start()
    {
        if(character != null)
        {
            if (character.group == Group.Player)
                target_tag = "Enemy";
            else if(character.group == Group.Enemy)
                target_tag = "Player";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == target_tag&&!character.isDeath)
        {
            character.noticed = other.gameObject;
            //character.agent.isStopped = false;
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > calculateInterval)
        {
            
        }
    }
}
