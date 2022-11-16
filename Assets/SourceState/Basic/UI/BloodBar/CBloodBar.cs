using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBloodBar : MonoBehaviour
{
    [Range(0,1)]
    public float value;
    public float energy;
    public Transform essence,essence2;
    public CharacterMovement character;

    private void Update()
    {
        value = character.health;
        energy = (character.energy - 0.4f) / 0.6f; ;
        if(energy<0)
            energy = 0;

        essence.localScale = new Vector3(value, essence.localScale.y, essence.localScale.z);
        essence.localPosition=new Vector3(0.5f- 0.5f * value, 0, 0);

        essence2.localScale = new Vector3(energy, essence2.localScale.y, essence2.localScale.z);
        essence2.localPosition = new Vector3(0.5f - 0.5f * energy, -0.02f, 0);

        transform.LookAt(Camera.main.transform.position);
    }
}
