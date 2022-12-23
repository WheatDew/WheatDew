using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMapTest : MonoBehaviour
{
    public Texture texture;
    public string currentTexture;
    private Bounds bounds;



    private void OnMouseOver()
    {
        

        if (Input.GetMouseButtonDown(1))
        {
            var material = GetComponent<MeshRenderer>().material;
            material.mainTexture = texture;
            currentTexture = "xiaoxi";
            float max_x = -999, max_y = -999, min_x = 999, min_y = 999;
            
            for(int i = 0; i < NewMapSystem.instance.memeber.Length; i++)
            {
                var item=NewMapSystem.instance.memeber[i];
                if(item.transform.position.x-0.5f<min_x)
                {
                    min_x = item.transform.position.x - 0.5f;
                }
                if (item.transform.position.x + 0.5f > max_x)
                {
                    max_x = item.transform.position.x + 0.5f;
                }
                if (item.transform.position.y - 0.5f < min_y)
                {
                    min_y = item.transform.position.y - 0.5f;
                }
                if (item.transform.position.y + 0.5f > max_y)
                {
                    max_y = item.transform.position.y + 0.5f;
                }
            }
            Vector3 center = new Vector3((max_x + min_x) / 2f, (min_y + max_y) / 2f,0);
            Vector2 length = new Vector2(max_x - min_x, max_y - min_y);
            center = new Vector3(center.x - length.x / 2f, center.y - length.y / 2f,0);
            material.mainTextureOffset = new Vector2((transform.position.x - center.x)/length.x, (transform.position.y - center.y) / length.y);
            material.mainTextureScale= new Vector2 (1f/length.x, 1f/length.y);
        }
    }
}
