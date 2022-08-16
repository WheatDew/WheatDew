using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Origin
{
    public class PackModule : MonoBehaviour
    {
        public PackSystem packSystem;

        private void Start()
        {
            TaskSystem.s.Declare("GainItem", GainItem);
            TaskSystem.s.Declare("PackItemUse", PackItemUseCommand);
        }

        public void GainItem(string[] value,TaskData task)
        {
            packSystem.PackItemGain(value[0], value[1], int.Parse(value[2])); 
        }

        public void PackItemUseCommand(string[] values,TaskData task)
        {
            if (packSystem.itemEffectList.ContainsKey(values[2]))
            {
                packSystem.PackItemGain(values[1], values[2], -1);
                StatusSystem.S.statusList[values[1]].FoodGain(packSystem.itemEffectList[values[2]].foodValue);
            }
        }
    }
}

