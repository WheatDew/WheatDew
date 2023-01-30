using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditorPage : MonoBehaviour
{
    public MapEditorElement elementPrefab;
    public Transform elementParent;

    public void CreateElement()
    {
        var obj = Instantiate(elementPrefab, elementParent);
    }
}
