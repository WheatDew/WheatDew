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
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickClosedItemJob();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            OpenRecipePage();
        }

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
}
