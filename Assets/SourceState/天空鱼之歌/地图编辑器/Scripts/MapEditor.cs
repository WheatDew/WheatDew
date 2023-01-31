using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEditor : MonoBehaviour
{
    private static MapEditor _instance;
    public static MapEditor instance { get { return _instance; } }
    

    private void Awake()
    {
        if( _instance == null)
        {
            _instance = this;
        }
    }


    [SerializeField] private MapEditorPage mapEditorPagePrefab;
    [HideInInspector] public static MapEditorPage mapEditorPage;

    [SerializeField] private MapEditorFloatElement mapEditorFloatElementPrefab;
    [HideInInspector] public static MapEditorFloatElement mapEditorFloatElement;

    public Transform pageParent;
    public Transform elementParent;
    public Transform elementGameObjectParent;

    public List<string> elementNames=new List<string>();
    public List<GameObject> elementPrefabs = new List<GameObject>();

    public Dictionary<string, GameObject> elementDict = new Dictionary<string, GameObject>();
    [HideInInspector] public GameObject currentElementGameObject;

    private void Start()
    {
        InitElementList();
        DisplayMapEditorPage();

    }

    public void InitElementList()
    {
        for(int i = 0; i < elementNames.Count; i++)
        {
            elementDict.Add(elementNames[i], elementPrefabs[i]);
        }
    }


    public void DisplayMapEditorPage()
    {
        mapEditorPage = Instantiate(mapEditorPagePrefab, pageParent);
    }

    public void CreateMapEditorFloatElement()
    {
        mapEditorFloatElement=Instantiate(mapEditorFloatElementPrefab,elementParent);
    }

    public void CreateMapEditorElementGameObject(string elementName,Vector3 position)
    {
        currentElementGameObject = Instantiate(elementDict[elementName]);
        currentElementGameObject.transform.position = position;
        currentElementGameObject.layer = 13;
    }
}
