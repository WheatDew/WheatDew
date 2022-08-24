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

    private void Start()
    {
        technologyDatas.Add("石质工具", new TechnologyData("石质工具", "石片加工"));
        technologyDatas.Add("石片加工", new TechnologyData("石片加工"));
        technologyDatas.Add("石质武器", new TechnologyData("石质武器","石片加工"));
        technologyDatas.Add("植物纤维加工", new TechnologyData("植物纤维加工", "石质工具"));
        technologyDatas.Add("长矛", new TechnologyData("长矛", "石质武器"));

        List<TechnologyData> roots = new List<TechnologyData>();


        List<List<string>> displayList = new List<List<string>>();

        displayList.Add(new List<string>());
        foreach (var item in technologyDatas)
        {
            if (item.Value.pretechnology.Count == 0)
            {
                displayList[0].Add(item.Value.technologyName);
                item.Value.level = 0;
            }
        }

        int levelCount = 0;

        for (int n = 1; n < 5; n++)
        {
            Debug.Log(n.ToString());
            displayList.Add(new List<string>());
            bool flag = false;
            foreach (var item in technologyDatas)
            {
                for (int i = 0; i < item.Value.pretechnology.Count; i++)
                {
                    if (displayList[n - 1].Contains(item.Value.pretechnology[i]))
                    {
                        item.Value.level = n;
                        if (!displayList[n].Contains(item.Value.technologyName))
                            displayList[n].Add(item.Value.technologyName);
                        if (!technologyDatas[item.Value.pretechnology[i]].postechnology.Contains(item.Value.technologyName))
                        {
                            technologyDatas[item.Value.pretechnology[i]].postechnology.Add(item.Value.technologyName);


                        }
                        flag = true;
                    }
                }
            }
            if (!false)
            {
                levelCount = n + 1;
                break;
            }

        }

        technologyPage = Instantiate(technologyPagePrefab, FindObjectOfType<Canvas>().transform);

        technologyPage.gridLayoutGroup.constraintCount = levelCount;

        int hightCount = 0;

        foreach (var item in technologyDatas)
        {
            if (item.Value.postechnology.Count > hightCount)
                hightCount = item.Value.postechnology.Count;
        }

        List<TechnologyItem> pageItemList = new List<TechnologyItem>();
        for (int i = 0; i < levelCount * hightCount; i++)
        {
            pageItemList.Add(technologyPage.Create(i.ToString()));
        }

        List<List<string>> temp = new List<List<string>>();
        for (int i = 0; i < levelCount; i++)
        {
            temp.Add(new List<string>());
        }

        foreach (var item in technologyDatas)
        {
            Debug.Log(string.Format("{0} {1}", item.Value.technologyName, item.Value.level));
            temp[item.Value.level].Add(item.Value.technologyName);
            
        }

        for(int i = 0; i < temp.Count; i++)
        {
            for(int j = 0; j < temp[i].Count; j++)
            {
                pageItemList[j * temp.Count + i].nameText.text = temp[i][j];
            }
        }
    }

}


public class TechnologyData
{
    public List<string> pretechnology=new List<string>();
    public List<string> postechnology=new List<string>();
    public string technologyName;
    public int level = 0;
    public Vector2 position;

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
