using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorDragComponent : MonoBehaviour
{
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit hitInfo))
        {
            transform.position=hitInfo.point;
        }
    }
}
