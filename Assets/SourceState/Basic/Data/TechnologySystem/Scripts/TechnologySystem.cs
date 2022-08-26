using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechnologySystem : MonoBehaviour
{
    private static TechnologySystem _s;
    public static TechnologySystem s { get { return _s; } }

    private void Awake()
    {
        if (_s == null) _s = this;
    }


    [SerializeField] private TechnologyPage technologyPagePrefab;
    [HideInInspector] public TechnologyPage technologyPage;
    public Dictionary<string, TechnologyData> technologyDatas = new Dictionary<string, TechnologyData>();

    public List<List<string>> displayList=new List<List<string>>();

    private void Start()
    {
        CreateTechnologyData("0a");
        CreateTechnologyData("1a","0a");
        CreateTechnologyData("1b", "0a");
        CreateTechnologyData("2a", "1a");
        CreateTechnologyData("2b", "1a");
        CreateTechnologyData("2c", "1a");
        CreateTechnologyData("2d", "1b");


        foreach (var item in technologyDatas)
        {
            for(int i = 0; i < item.Value.pretechnology.Count; i++)
            {
                technologyDatas[item.Value.pretechnology[i]].postechnology.Add(item.Value.technologyName);
            }
        }

        GetNodeData("0a", 0);

        foreach(var item in technologyDatas)
        {
            Debug.Log(string.Format("{0} {1}", item.Value.technologyName,item.Value.level));
        }

        int max=0;
        //获取列表中的最大长度
        for(int i = 0; i < displayList.Count; i++)
        {
            if (displayList[i].Count > max)
            {
                max=displayList[i].Count;
            }
        }

        technologyPage = Instantiate(technologyPagePrefab, FindObjectOfType<Canvas>().transform);
        technologyPage.gridLayoutGroup.constraintCount=displayList.Count;

        List<TechnologyItem> pageItemList = new List<TechnologyItem>();
        for (int i = 0; i < displayList.Count * max; i++)
        {
            pageItemList.Add(technologyPage.Create(""));
        }

        for (int i = 0; i < displayList.Count; i++)
        {
            for (int j = 0; j < displayList[i].Count; j++)
            {
                pageItemList[j * displayList.Count + i].nameText.text = displayList[i][j];
                pageItemList[j * displayList.Count + i].background.gameObject.SetActive(true);
                technologyDatas[displayList[i][j]].item = pageItemList[j * displayList.Count + i];

                int startIndex = 0;
                if(technologyDatas[displayList[i][j]].postechnology.Count!=0)
                    startIndex = displayList[i + 1].IndexOf(technologyDatas[displayList[i][j]].postechnology[0]);

                for (int index=j+1; index < startIndex; index++)
                {
                    pageItemList[index * displayList.Count + i + 1].bridgeLine.SetActive(true);
                }

                for (int n = 0; n < technologyDatas[displayList[i][j]].postechnology.Count; n++)
                {
                    if (n == 0&& startIndex==0)
                    {
                        pageItemList[j * displayList.Count + i].backLine.SetActive(true);
                        pageItemList[j * displayList.Count + i+1].frontLine.SetActive(true);
                    }
                    else
                    {
                        if(n>=1&& technologyDatas[displayList[i][j]].postechnology.Count > 2)
                        {
                            pageItemList[(n+startIndex) * displayList.Count + i + 1].bridgeLine.SetActive(true);
                        }

                        Debug.Log(string.Format("{0} {1} {2}",j, n, startIndex));
                        pageItemList[j * displayList.Count + i].backPolyline.SetActive(true);
                        pageItemList[(n+startIndex) * displayList.Count + i+1].frontPolyline.SetActive(true);
                    }
                }

            }
        }

    }

    //遍历树节点
    public void GetNodeData(string name,int level)
    {
        technologyDatas[name].level = level;
        if (displayList.Count>level)
        {
            displayList[level].Add(name);
        }
        else
        {
            displayList.Add(new List<string>());
            displayList[level].Add(name);
        }
        for(int i = 0; i < technologyDatas[name].postechnology.Count; i++)
        {
            GetNodeData(technologyDatas[name].postechnology[i], level+1);
        }
    }

    //添加条目
    public void CreateTechnologyData(string name,params string[] pretechnology)
    {
        technologyDatas.Add(name, new TechnologyData(name, pretechnology));
    }
}


public class TechnologyData
{
    public List<string> pretechnology=new List<string>();
    public List<string> postechnology=new List<string>();
    public string technologyName;
    public int level = -1;
    public TechnologyItem item;

    public TechnologyData(string technologyName,params string[] pretechnology)
    {
        this.technologyName = technologyName;
        for (int i = 0; i < pretechnology.Length; i++)
        {
            this.pretechnology.Add(pretechnology[i]);
        }

    }

    public TechnologyData(string technologyName, List<string> pretechnology,List<string> postechnology)
    {
        this.pretechnology = new List<string>(pretechnology);
        this.postechnology = new List<string>(postechnology);
        this.technologyName = technologyName;
    }

    public TechnologyData SetPretechnology(params string[] pretechnology)
    {
        for(int i = 0; i < pretechnology.Length; i++)
        {
            this.pretechnology.Add(pretechnology[i]);
        }
        return this;
    }

    public void AddPostechnology(string postechnologyName)
    {
        this.postechnology.Add(postechnologyName);
    }

    public void SetPostechnology(params string[] postechnology)
    {
        for (int i = 0; i < postechnology.Length; i++)
        {
            this.pretechnology.Add(pretechnology[i]);
        }
    }
}
