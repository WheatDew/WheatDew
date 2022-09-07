using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;

public class CCharacter : MonoBehaviour
{
    private CEntity centity;

    public string commandTriggerPressK;

    private void Start()
    {
        centity = GetComponent<CEntity>();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            
        }
    }
}
