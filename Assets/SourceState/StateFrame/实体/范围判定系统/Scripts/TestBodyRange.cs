using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBodyRange : MonoBehaviour
{
    private float p = 0.0001f;
    public CharacterMovement character;
    public void OnTriggerEnter(Collider other)
    {

        if (!character.isDeath && other.tag == "Weapon" && other != character.weapon.box)
        {
            AnimatorStateInfo stateInfo= character.anim.GetCurrentAnimatorStateInfo(0);

            Debug.Log(other.name);

            if (character.isGuard)
            {
                character.anim.SetTrigger("GuardHit");
                character.health -= 0.02f+p;
                character.energy -= 0.3f;
            }
            else if (!stateInfo.IsName("Hit"))
            {
                character.anim.SetTrigger("Hit");
                character.health -= 0.2f + p;
            }

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
            else if (character.energy <= 0)
            {
                character.anim.ResetTrigger("Attack");
                character.anim.ResetTrigger("Dodge");
                character.anim.SetTrigger("GuardBreak");
            }


        }
    }
}
