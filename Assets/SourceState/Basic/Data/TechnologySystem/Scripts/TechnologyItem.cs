using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TechnologyItem : MonoBehaviour,IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    public TMP_Text nameText;
    public GameObject frontLine, backLine,frontPolyline,backPolyline,bridgeLine;
    public Image background;
    public Color press;
    public Color normal;


    public void OnPointerDown(PointerEventData eventData)
    {
        if(Input.GetMouseButton(0))
        {
            
            foreach (var item in TechnologySystem.s.technologyDatas)
            {
                item.Value.item.background.color = normal;
            }
            background.color = press;
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        
    }
}
