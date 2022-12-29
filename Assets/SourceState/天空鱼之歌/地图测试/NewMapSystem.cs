using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NewMapSystem : MonoBehaviour
{
    private static NewMapSystem _instance;
    public static NewMapSystem instance { get { return _instance; } }


    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
        }
    }

    public Texture[] incoList;
    public MapElement[] memeber;
    public List<Material> flashMaterialList = new List<Material>();

    float value = 0;
    float direct = 1;

    public void Flash(Material material)
    {
        flashMaterialList.Add(material);
    }

    private void Update()
    {
        value += Time.deltaTime * direct;
        if (value > 1)
        {
            direct = -1;
        }
        if (value < 0)
        {
            direct = 1;
        }

        for(int i = 0; i < flashMaterialList.Count; i++)
        {
            flashMaterialList[i].SetColor("_BaseColor0", Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 0.5f, 0.5f, 1), value));
            flashMaterialList[i].SetTexture("_BaseColorMap1", incoList[0]);
            flashMaterialList[i].SetColor("_BaseColor1", Color.Lerp(new Color(1,0,0,0), new Color(1,0,0,0.1f), value));
        }
    }
}
