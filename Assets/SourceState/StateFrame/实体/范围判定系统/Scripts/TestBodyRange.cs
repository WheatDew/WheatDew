using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBodyRange : MonoBehaviour
{
    public CharacterMovement character;
    public void OnTriggerEnter(Collider other)
    {

        if (!character.isDeath && other.tag == "Weapon" && other.gameObject != character.weapon)
        {
            Debug.Log(other.name);
            if (!character.anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                character.anim.SetTrigger("Death");
            character.isDeath = true;
            if (character.agent != null)
                character.agent.isStopped = true;
            character.tag = "Death";
        }
    }
}
