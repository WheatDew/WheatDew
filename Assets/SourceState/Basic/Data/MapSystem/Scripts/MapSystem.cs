using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSystem : MonoBehaviour
{
    public List<GameObject> mapItems = new List<GameObject>();
    public List<int> mapItemCounts = new List<int>();
    public Terrain terrain;

    private void Start()
    {
        int length = Mathf.RoundToInt(terrain.terrainData.size.x);
        int width = Mathf.RoundToInt(terrain.terrainData.size.z);
        Debug.Log(terrain.terrainData.size);
        for (int i = 0; i < mapItems.Count; i++)
        {
            for(int n = 0; n < mapItemCounts[i]; n++)
            {
                int x = Random.Range(0, length);
                int z = Random.Range(0, width);
                float y = terrain.SampleHeight(new Vector3(x,0,z));
                GameObject obj = Instantiate(mapItems[i]);
                obj.transform.position = new Vector3(x,y,z);
            }

            
        }
    }
}
