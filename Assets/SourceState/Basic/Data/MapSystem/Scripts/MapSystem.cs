using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    public List<GameObject> mapItems = new List<GameObject>();
    public List<int> mapItemCounts = new List<int>();
    public Terrain terrain;

    public Dictionary<string,MapItemData> mapDatas = new Dictionary<string,MapItemData>();

    private void Start()
    {
        Init();
        SetMapItem();
        CreateMapItem();
    }

    public void Init()
    {
        foreach (var item in mapItems)
        {
            mapDatas.Add(item.name, new MapItemData(item));
        }
    }

    public void SetMapItem()
    {
        SetItem("Ê÷Ö¦1").SetCount(200).SetRotation(0,360).SetSize(0.8f,1.2f);
        SetItem("Ê÷Ö¦2").SetCount(200).SetRotation(0, 360).SetSize(0.8f, 1.2f);
        SetItem("Ê÷Ö¦3").SetCount(200).SetRotation(0, 360).SetSize(0.8f, 1.2f);
        SetItem("Ê÷Ö¦4").SetCount(200).SetRotation(0, 360).SetSize(0.8f, 1.2f);
        SetItem("Ê÷Ö¦5").SetCount(200).SetRotation(0, 360).SetSize(0.8f, 1.2f);
        SetItem("Ê¯Í·").SetCount(1000).SetRotation(0, 360).SetSize(0.8f, 1.2f);
        SetItem("ÑîÊ÷1").SetCount(1000).SetRotation(0, 360).SetSize(0.8f, 1.2f);
    }

    public void CreateMapItem()
    {
        int length = Mathf.RoundToInt(terrain.terrainData.size.x);
        int width = Mathf.RoundToInt(terrain.terrainData.size.z);

        foreach(var item in mapDatas)
        {
            for(int i = 0; i < Random.Range(item.Value.minCount,item.Value.maxCount); i++)
            {
                int x = Random.Range(0, length);
                int z = Random.Range(0, width);
                float y = terrain.SampleHeight(new Vector3(x, 0, z));
                GameObject obj = Instantiate(item.Value.prefab);
                obj.transform.position = new Vector3(x, y, z);
                obj.transform.eulerAngles=new Vector3(0,Random.Range(item.Value.minRotation,item.Value.MaxRotation),0);
                obj.transform.localScale= Vector3.one * Random.Range(item.Value.minSize, item.Value.maxSize);
            }
        }

    }

    public MapItemData SetItem(string item)
    {
        if (mapDatas.ContainsKey(item))
        {
            return mapDatas[item];
        }
        return null;
    }
}

public class MapItemData
{
    public GameObject prefab;
    public int minCount=0,maxCount=0;
    public float minSize=1,maxSize=1,minRotation=0,MaxRotation=0;

    public MapItemData(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public MapItemData SetPrefab(GameObject prefab)
    {
        this.prefab = prefab;
        return this;
    }
    public MapItemData SetCount(int min,int max)
    {
        minCount = min;
        maxCount = max;
        return this;
    }

    public MapItemData SetCount(int count)
    {
        minCount = count;
        maxCount = count;
        return this;
    }

    public MapItemData SetSize(float min, float max)
    {
        minSize = min;
        maxSize = max;
        return this;
    }

    public MapItemData SetSize(float value)
    {
        minSize = value;
        maxSize = value;
        return this;
    }

    public MapItemData SetRotation(float min, float max)
    {
        minRotation = min;
        MaxRotation = max;
        return this;
    }

    public MapItemData SetRotation(float value)
    {
        minRotation = value;
        MaxRotation = value;
        return this;
    }
}
