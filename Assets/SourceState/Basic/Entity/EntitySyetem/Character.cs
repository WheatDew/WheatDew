using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Origin;

public class Character : MonoBehaviour
{
    private string key;
    private RangeComponent range;

    private void Start()
    {
        key = transform.GetInstanceID().ToString();
        range = GetComponent<RangeComponent>();
    }

    private void Update()
    {
        ResetBuilding();

        if (Input.GetKeyDown(KeyCode.E))
        {
            PickClosedItemJob();
        }

        //if (Input.GetKeyDown(KeyCode.F))
        //{
        //    OpenRecipePage();
        //}


    }


    public InfoData PickClosedItemJob()
    {
        TaskSystem.s.Execute(string.Format("PickItem {0}", key));

        return null;
    }

    public void OpenRecipePage()
    {
        string subject = "";
        foreach(var item in range.currentRange)
        {
            if (PackSystem.S.components.ContainsKey(item) && PackSystem.S.components[item].recipes!="")
                subject = item;
        }
        if(subject!="")
            TaskSystem.s.Execute(string.Format("OpenRecipePage {0} {1}", subject, key));
    }

    //重新放置建筑
    public async void ResetBuilding()
    {

        string subject = "";
        if (Input.GetKeyDown(KeyCode.E))
        {
            float timer = 0;
            while (timer < 1)
            {
                Debug.Log("计算重新放置");
                if (Input.GetKeyUp(KeyCode.E))
                {
                    return;
                }
                timer += Time.deltaTime;
                await new WaitForSeconds(Time.deltaTime);
            }

            BuildingSystem s_building = BuildingSystem.S;

            foreach (var item in range.currentRange)
            {
                Debug.Log(item);
                if (s_building.buildings.ContainsKey(item))
                    subject = item;
            }
            if (subject != "")
            {
                //s_building.CreateBuildingBluePrint(s_building.buildings[subject].buildingTypeName);
                //s_building.DestroyBuilding(subject);

                Transform target = s_building.buildings[subject].transform;
                RaycastHit result;
                while (true)
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if(Physics.Raycast(ray,out result, 100, 1 << 7))
                    {
                        target.position = result.point-0.2f*Vector3.up;
                    }
                    if (Input.GetMouseButtonDown(0))
                    {
                        return;
                    }
                    await new WaitForUpdate();
                }
            }

        }
    } 
}
