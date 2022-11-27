using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTask : MonoBehaviour
{
    public List<CBuilding> buildings;

    public Stack<CBuilding> currentBuildingTask = new Stack<CBuilding>();


    public void AddBuilding(CBuilding cBuilding)
    {
        currentBuildingTask.Push(cBuilding);
    }




    private void Update()
    {


        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            buildings[0].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            buildings[1].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            buildings[2].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            buildings[3].gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            buildings[4].gameObject.SetActive(true);
        }

        if (currentBuildingTask.Count != 0)
        {
            var building = currentBuildingTask.Pop();

        }

        
    }
}
