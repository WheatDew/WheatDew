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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            PickClosedItemJob();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            
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
        TaskSystem.s.Execute(string.Format("OpenRecipePage {0} {1}", subject, key));
    }
}
