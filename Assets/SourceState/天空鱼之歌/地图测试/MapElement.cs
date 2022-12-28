using UnityEngine;

public class MapElement : MonoBehaviour
{
    public Texture texture;
    public string currentTexture;
    private Bounds bounds;
    [System.NonSerialized] public Material material;
    public enum CenterType { Point,Constant}
    public CenterType centerType;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        bounds = transform.GetComponent<Collider>().bounds;
    }

    public void SetMapElement(string faith)
    {
        
    }

    private void OnMouseOver()
    {
        

        if (Input.GetMouseButtonDown(1))
        {
            material.mainTexture = texture;
            currentTexture = "xiaoxi";
            float max_x = -999, max_y = -999, min_x = 999, min_y = 999;
            
            for(int i = 0; i < NewMapSystem.instance.memeber.Length; i++)
            {
                var item=NewMapSystem.instance.memeber[i];
                if (item.currentTexture != "xiaoxi")
                    continue;
                if(item.bounds.min.x<min_x)
                {
                    min_x = item.bounds.min.x;
                }
                if (item.bounds.max.x > max_x)
                {
                    max_x = item.bounds.max.x;
                }
                if (item.bounds.min.y < min_y)
                {
                    min_y = item.bounds.min.y;
                }
                if (item.bounds.max.y > max_y)
                {
                    max_y = item.bounds.max.y;
                }
            }

            if (centerType == CenterType.Constant)
            {
                Vector3 center = new Vector3((max_x + min_x) / 2f, (min_y + max_y) / 2f, 0);
                Vector2 length = new Vector2(max_x - min_x, max_y - min_y);
                center = new Vector3(center.x - length.x / 2f, center.y - length.y / 2f, 0);

                for (int i = 0; i < NewMapSystem.instance.memeber.Length; i++)
                {
                    var item = NewMapSystem.instance.memeber[i];
                    if (item.currentTexture != "xiaoxi")
                        continue;

                    item.material.mainTextureOffset = new Vector2((item.transform.position.x - 1f - center.x) / length.x, (item.transform.position.y - 1f - center.y) / length.y);
                    item.material.mainTextureScale = new Vector2(2f / length.x, 2f / length.y);
                }
            }
            else if(centerType == CenterType.Point)
            {
                Debug.DrawLine(new Vector3(min_x, min_y, 0.1f), new Vector3(max_x, min_y, 0.1f), Color.red, 10);
                Debug.DrawLine(new Vector3(max_x, min_y, 0.1f), new Vector3(max_x, max_y, 0.1f), Color.red, 10);
                Debug.DrawLine(new Vector3(max_x, max_y, 0.1f), new Vector3(min_x, max_y, 0.1f), Color.red, 10);
                Debug.DrawLine(new Vector3(min_x, max_y, 0.1f), new Vector3(min_x, min_y, 0.1f), Color.red, 10);
                Vector3 center = new Vector3((max_x + min_x) / 1f, (min_y + max_y) / 1f, 0);
                Vector2 length = new Vector2(max_x - min_x, max_y - min_y);
                center = new Vector3(center.x - length.x / 1f, center.y - length.y / 1f, 0);

                for (int i = 0; i < NewMapSystem.instance.memeber.Length; i++)
                {
                    var item = NewMapSystem.instance.memeber[i];
                    if (item.currentTexture != "xiaoxi")
                        continue;

                    item.material.mainTextureOffset = new Vector2((item.transform.position.x - center.x) / length.x, (item.transform.position.y+0.5f - center.y) / length.y);
                    item.material.mainTextureScale = new Vector2(1f / length.x, 1f / length.y);
                }
            }

        }
    }
}
