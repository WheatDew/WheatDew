using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRange : MonoBehaviour
{
    public Suntail.Door door;

    private HashSet<GameObject> players = new HashSet<GameObject>();

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)&&players.Count>0)
            door.PlayDoorAnimation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            players.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (players.Contains(other.gameObject))
        {
            players.Remove(other.gameObject);
        }
    }
}
