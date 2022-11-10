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

            if (character.isGuard)
            {
                character.anim.SetTrigger("GuardHit");
            }
            else if (!character.anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            {
                character.anim.SetTrigger("Hit");
            }


            character.health -= 0.21f;



            if(character.health<=0)
            {
                character.health = 0;
                character.isDeath = true;
                if (character.agent != null)
                    character.agent.isStopped = true;
                character.tag = "Death";
                if (!character.anim.GetCurrentAnimatorStateInfo(0).IsName("Death"))
                    character.anim.SetTrigger("Death");
            }

        }
    }
}
