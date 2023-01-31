using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorPage : MonoBehaviour
{
    public MapEditorElement elementPrefab;
    public Transform elementParent;

    [HideInInspector] public RectTransform rectTransform;
    [HideInInspector] public Vector2 pos,size;
    [HideInInspector] public Vector2 bound_x,bound_y;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        pos=rectTransform.position;
        size = rectTransform.sizeDelta;
        bound_x = new Vector2(pos.x - size.x*0.5f, pos.x + size.x*0.5f);
        bound_y = new Vector2(pos.y - size.y*0.5f, pos.y + size.y*0.5f);
        CreateElement();
        CreateElement();
    }

    public void CreateElement()
    {
        var obj = Instantiate(elementPrefab, elementParent);
    }
}
