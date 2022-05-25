using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Origin
{
    public class PackPageItem : MonoBehaviour, IPointerDownHandler
    {
        public string target;
        public Text itemName, itemCount;
        public List<CommandButton> commandButtons;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                PackSystem.S.CreateItemRightMenu(target,this,commandButtons);
            }
        }
    }
}

