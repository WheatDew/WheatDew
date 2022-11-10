using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBloodBar : MonoBehaviour
{
    [Range(0,1)]
    public float value;
    public Transform mask;
    public CharacterMovement character;

    private void Update()
    {
        value = character.health;
        if (value < 0)
        {
            value = 0;
        }
        mask.localPosition = new Vector3(-value, 0);
        transform.LookAt(Camera.main.transform.position);
    }
}
