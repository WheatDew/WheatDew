using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PickModule : MonoBehaviour
    {
        public PickSystem pickSystem;

        private void Start()
        {
            TaskSystem.s.Declare("PickItem", PickItem);
        }

        public void PickItem(string[] value,TaskData taskData)
        {
            bool flag = false;
            HashSet<string> items = RangeSystem.s.components[value[1]].currentRange;
            foreach (string item in items)
            {
                if (EntitySystem.s.components[item].itemName != ""&& RangeSystem.s.components[item].rangeType==RangeType.Item)
                {
                    EntitySystem.s.DestroyEntity(item);
                    PackSystem.S.PackItemGain(value[1], EntitySystem.s.components[item].itemName, EntitySystem.s.components[item].itemCount);

                    flag = true;
                }
            }

            if (flag)
            {
                items.Clear();
                InfoInSceneSystem.s.HiddenItemInfoInScene();
            }



        }
    }

}

