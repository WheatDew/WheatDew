using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TechnologyPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public RectTransform target;
    private bool isOver=false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        isOver=true;

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isOver =false;
    }

    private void Update()
    {
        if (isOver && Input.GetMouseButton(2))
        {
            target.position += new Vector3(Input.GetAxisRaw("Mouse X")*100, Input.GetAxisRaw("Mouse Y")*100, 0);
        }
    }
}
