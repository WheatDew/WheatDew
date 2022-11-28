using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRange : MonoBehaviour
{
    public Suntail.Door door;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            door.PlayDoorAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
