using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathFindingTest : MonoBehaviour
{
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit result;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out result))
            {
                agent.destination = result.point;
            }
        }

        for(int i = 2; i < agent.path.corners.Length; i++)
        {
            Debug.DrawLine(agent.path.corners[i], agent.path.corners[i-1]);
        }
    }
}
