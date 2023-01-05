using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public enum CenterType { Point, Constant }
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
    public Dictionary<string,Texture> faithPictureList=new Dictionary<string, Texture>();
    public List<string> faithNameList=new List<string>();

    float value = 0;
    float direct = 1;

    private async void Start()
    {
        UpdateFaithPictureList(Application.streamingAssetsPath + "/FaithPictures");
        await new WaitForUpdate();
        UpdateFaithTexture();
    }

    public void Attack(MapElement mapElement,string faith)
    {
        if (flashMaterialList.Contains(mapElement.material))
        {
            flashMaterialList.Remove(mapElement.material);

            mapElement.material.SetTexture("_BaseColorMap0", null);
            mapElement.material.SetColor("_BaseColor0", Color.white);
            mapElement.material.SetTexture("_BaseColorMap1", null);
            mapElement.material.SetColor("_BaseColor1", new Color(1,0,0,0));

            mapElement.faith = faith;
            UpdateFaithTexture();
        }
        else
        {
            flashMaterialList.Add(mapElement.material);
        }

    }


    public void ReadFaithTexture()
    {
        //GetTextureByByte(SetImageToByte(string.Format( Application.streamingAssetsPath))
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

    /// <summary>
    /// 将图片转换为字节数组
    /// </summary>
    public byte[] SetImageToByte(string imgPath)
    {
        FileStream fs = new FileStream(imgPath, FileMode.Open);
        byte[] imgByte = new byte[fs.Length];
        fs.Read(imgByte, 0, imgByte.Length);
        fs.Close();
        return imgByte;
    }

    /// <summary>
    /// 将字节数组转换为纹理    
    /// </summary>
    public Texture GetTextureByByte(byte[] imgByte)
    {
        Texture2D tex = new Texture2D(1900, 1900);
        tex.LoadImage(imgByte);
        return tex;
    }

    public void UpdateFaithPictureList(string path)
    {
        foreach (var item in Directory.GetFiles(path, "*.png"))
        {

            faithPictureList.Add(item.Split('\\')[1][..^4], GetTextureByByte(SetImageToByte(item)));
        }
        foreach (var item in Directory.GetFiles(path, "*.bmp"))
        {

            faithPictureList.Add(item.Split('\\')[1][..^4], GetTextureByByte(SetImageToByte(item)));
        }

    }

    //设置贴图
    public void UpdateFaithTexture()
    {
        for (int i = 0; i < memeber.Length; i++)
        {
            if (faithPictureList.ContainsKey(memeber[i].faith))
            {
                memeber[i].material.SetTexture("_BaseColorMap0", faithPictureList[memeber[i].faith]);
                if (!faithNameList.Contains(memeber[i].faith))
                {
                    faithNameList.Add(memeber[i].faith);
                }
            }

        }
        
        //绘制整体图片
        for(int n=0;n<faithNameList.Count; n++)
        {
            //计算公共参数
            float max_x = -999, max_z = -999, min_x = 999, min_z = 999;

            for (int i = 0; i < memeber.Length; i++)
            {
                
                if (memeber[i].faith == faithNameList[n])
                {
                    var item = memeber[i];
                    if (item.bounds.min.x < min_x)
                    {
                        min_x = item.bounds.min.x;
                    }
                    if (item.bounds.max.x > max_x)
                    {
                        max_x = item.bounds.max.x;
                    }
                    if (item.bounds.min.z < min_z)
                    {
                        min_z = item.bounds.min.z;
                    }
                    if (item.bounds.max.z > max_z)
                    {
                        max_z = item.bounds.max.z;
                    }
                }
                
            }

            //Debug.DrawLine(new Vector3(min_x,0.01f , min_z), new Vector3(max_x,0.01f, min_z), Color.red, 1000);
            //Debug.DrawLine(new Vector3(max_x,0.01f, min_z), new Vector3(max_x,0.01f, max_z), Color.red, 1000);
            //Debug.DrawLine(new Vector3(max_x,0.01f, max_z), new Vector3(min_x,0.01f ,max_z), Color.red, 1000);
            //Debug.DrawLine(new Vector3(min_x,0.01f, max_z), new Vector3(min_x,0.01f ,min_z), Color.red, 1000);

            Vector3 center = new Vector3((max_x + min_x) / 2f,0 , (min_z + max_z) / 2f);
            Vector2 length = new Vector2(max_x - min_x, max_z - min_z);
            //center = new Vector3(center.x - length.x / 2f, 0 , center.z - length.y / 2f);

            for (int i = 0; i < memeber.Length; i++)
            {
                if(memeber[i].faith == faithNameList[n])
                {
                    var item = memeber[i];

                    item.material.SetTextureOffset("_BaseColorMap0", new Vector2((2*center.x +  length.x-2)/(2*length.x), (2 * center.z + length.y - 2) /(2 * length.y)));
                    item.material.SetTextureScale("_BaseColorMap0", new Vector2(2f / length.x, 2f / length.y));
                    //item.material.SetTextureScale("_BaseColorMap0", new Vector2(1, 1));
                }

            }

            //Debug.DrawLine(new Vector3(min_x, min_y, -0.1f), new Vector3(max_x, min_y, -0.01f), Color.red, 10);
            //Debug.DrawLine(new Vector3(max_x, min_y, -0.1f), new Vector3(max_x, max_y, -0.01f), Color.red, 10);
            //Debug.DrawLine(new Vector3(max_x, max_y, -0.1f), new Vector3(min_x, max_y, -0.01f), Color.red, 10);
            //Debug.DrawLine(new Vector3(min_x, max_y, -0.1f), new Vector3(min_x, min_y, -0.01f), Color.red, 10);

            //Vector3 center = new Vector3((max_x + min_x) / 2f, (min_y + max_y) / 2f, 0);
            //Vector2 length = new Vector2(max_x - min_x, max_y - min_y);
            //center = new Vector3(center.x - length.x / 2f, center.y - length.y / 2f, 0);

            //for (int i = 0; i < memeber.Length; i++)
            //{

            //    if (memeber[i].faith == faithNameList[n])
            //    {
            //        var item = memeber[i];
            //        item.material.SetTextureOffset("_BaseColorMap0", new Vector2((item.transform.position.x - 0.5f - center.x) / length.x, (item.transform.position.y - 0.5f - center.y) / length.y));
            //        item.material.SetTextureScale("_BaseColorMap0", new Vector2(1f / length.x, 1f / length.y));
            //    }

            //}
        }
    }

    public void SetMapElement(string faith,MapElement mapElement)
    {
        mapElement.material.SetTexture("_BaseColorMap0", NewMapSystem.instance.faithPictureList[faith]);
        mapElement.faith = faith;
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

        if (mapElement.centerType == CenterType.Constant)
        {

            Vector3 center = new Vector3((max_x + min_x) / 2f, (min_y + max_y) / 2f, 0);
            Vector2 length = new Vector2(max_x - min_x, max_y - min_y);
            center = new Vector3(center.x - length.x / 2f, center.y - length.y / 2f, 0);

            for (int i = 0; i < memeber.Length; i++)
            {
                var item = memeber[i];
                if (item.faith != faith)
                    continue;

                item.material.mainTextureOffset = new Vector2((item.transform.position.x - 1f - center.x) / length.x, (item.transform.position.y - 1f - center.y) / length.y);
                item.material.mainTextureScale = new Vector2(2f / length.x, 2f / length.y);
            }
        }
        else if (mapElement.centerType == CenterType.Point)
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
}
