using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    [SerializeField] private MapEditorPage mapEditorPagePrefab;
    [HideInInspector] public static MapEditorPage mapEditorPage;

    public Transform pageParent;

    private void Start()
    {
        DisplayMapEditorPage();

    }

    public void DisplayMapEditorPage()
    {
        mapEditorPage = Instantiate(mapEditorPagePrefab, pageParent);
    }


}
