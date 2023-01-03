using UnityEngine;

public class MapElement : MonoBehaviour
{
    public string faith;
    [System.NonSerialized] public Bounds bounds;
    [System.NonSerialized] public Material material;

    public CenterType centerType;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        bounds = transform.GetComponent<Collider>().bounds;
    }

    public void SetMapElement(string faith)
    {
        Debug.Log(faith.Length);
        material.SetTexture("_BaseColorMap0", NewMapSystem.instance.faithPictureList[faith]);
        this.faith = faith;
        float max_x = -999, max_y = -999, min_x = 999, min_y = 999;

        for (int i = 0; i < NewMapSystem.instance.memeber.Length; i++)
        {
            var item = NewMapSystem.instance.memeber[i];
            if (item.faith != faith)
                continue;
            if (item.bounds.min.x < min_x)
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
                if (item.faith != faith)
                    continue;

                item.material.mainTextureOffset = new Vector2((item.transform.position.x - 1f - center.x) / length.x, (item.transform.position.y - 1f - center.y) / length.y);
                item.material.mainTextureScale = new Vector2(2f / length.x, 2f / length.y);
            }
        }
        else if (centerType == CenterType.Point)
        {
            Debug.DrawLine(new Vector3(min_x, min_y, 0.1f), new Vector3(max_x, min_y, 0.1f), Color.red, 10);
            Debug.DrawLine(new Vector3(max_x, min_y, 0.1f), new Vector3(max_x, max_y, 0.1f), Color.red, 10);
            Debug.DrawLine(new Vector3(max_x, max_y, 0.1f), new Vector3(min_x, max_y, 0.1f), Color.red, 10);
            Debug.DrawLine(new Vector3(min_x, max_y, 0.1f), new Vector3(min_x, min_y, 0.1f), Color.red, 10);
            Vector3 center = new Vector3((max_x + min_x) / 2f, (min_y + max_y) / 2f, 0);
            Vector2 length = new Vector2(max_x - min_x, max_y - min_y);
            center = new Vector3(center.x - length.x / 2f, center.y - length.y / 2f, 0);

            for (int i = 0; i < NewMapSystem.instance.memeber.Length; i++)
            {
                var item = NewMapSystem.instance.memeber[i];
                if (item.faith != faith)
                    continue;
                item.material.SetTextureOffset("_BaseColorMap0", new Vector2((item.transform.position.x - 0.5f - center.x) / length.x, (item.transform.position.y - 0.5f - center.y) / length.y));
                item.material.SetTextureScale("_BaseColorMap0", new Vector2(1f / length.x, 1f / length.y));
            }
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NewMapSystem.instance.Flash(material);
        }
        

        if (Input.GetMouseButtonDown(1))
        {
            SetMapElement("小希");

        }
    }
}
