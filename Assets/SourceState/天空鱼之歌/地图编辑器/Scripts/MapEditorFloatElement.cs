using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorFloatElement : MonoBehaviour
{

    public RectTransform rectTransform;

    private void Start()
    {
        
    }

    private void Update()
    {
        rectTransform.position = Input.mousePosition;

        if (rectTransform.position.x > MapEditor.mapEditorPage.bound_x.y
         || rectTransform.position.x < MapEditor.mapEditorPage.bound_x.x
         || rectTransform.position.y > MapEditor.mapEditorPage.bound_y.y
         || rectTransform.position.y < MapEditor.mapEditorPage.bound_y.x)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit result;
            if(Physics.Raycast(ray, out result))
            {
                if (MapEditor.instance.currentElementGameObject == null)
                    MapEditor.instance.CreateMapEditorElementGameObject("ľͰ", result.point);
                else
                    MapEditor.instance.currentElementGameObject.transform.position = result.point;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Destroy(gameObject);
        }
    }
}
