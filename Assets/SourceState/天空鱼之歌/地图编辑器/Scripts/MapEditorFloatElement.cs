using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapEditorFloatElement : MonoBehaviour
{
    private Image image;
    public RectTransform rectTransform;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    private void Update()
    {
        rectTransform.position = Input.mousePosition;

        if (rectTransform.position.x > MapEditor.mapEditorPage.bound_x.y
         || rectTransform.position.x < MapEditor.mapEditorPage.bound_x.x
         || rectTransform.position.y > MapEditor.mapEditorPage.bound_y.y
         || rectTransform.position.y < MapEditor.mapEditorPage.bound_y.x)
        {
            image.enabled = false;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit result;
            LayerMask layerMask = 1<<13;

            if (Physics.Raycast(ray, out result,100,~layerMask))
            {

                if (MapEditor.instance.currentElementGameObject == null)
                    MapEditor.instance.CreateMapEditorElementGameObject("木桶", result.point);
                else
                    MapEditor.instance.currentElementGameObject.transform.position = result.point;

            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            MapEditor.instance.currentElementGameObject = null;
            Destroy(gameObject);
        }
    }
}
