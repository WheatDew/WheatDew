using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class RDialogue : MonoBehaviour
{
    [SerializeField] private PDialogue dialoguePagePrefab;
    [HideInInspector] public PDialogue dialoguePage;

    private Dictionary<string,EventDialogue> events=new Dictionary<string, EventDialogue>();

    public EventDialogue currentEvent;
    public string currentCharacter="";
    public int currentIndex;


    private void Start()
    {
        GetFiles();
        SCommand.Declare(@"设置 事件 为 \S+?", SetCurrentEvent);
        SCommand.Declare(@"显示 下一句", SetNextContent);
        SCommand.Execute("设置 事件 为 小希故事1");

    }


    public void SetContent(string value,CommandData commandData)
    {
        string[] values = value.Split(' ');
        if (dialoguePage == null)
        {
            dialoguePage = Instantiate(dialoguePagePrefab, FindObjectOfType<Canvas>().transform);
        }
        dialoguePage.content.text = values[1];
    }

    public void SetNextContent(string value,CommandData commandData)
    {
        if (dialoguePage == null)
        {
            dialoguePage = Instantiate(dialoguePagePrefab, FindObjectOfType<Canvas>().transform);
        }
        currentIndex++;
        if (currentIndex < currentEvent.content.Count-1)
            SetDialogueText(currentEvent.content[currentIndex]);
        else
            Destroy(dialoguePage.gameObject);
    }

    public void SetCurrentEvent(string value, CommandData commandData)
    {
        string[] values=value.Split(' ');
        SetEventWithDisplay(values[3]);
        
    }

    public void CloseDialogue(string value,CommandData commandData)
    {
        Destroy(dialoguePage.gameObject);
    }

    //功能

    //文件读取
    public void ReadEventFile(string fileName)
    {
        string eventText= File.ReadAllText(fileName);
        EventDialogue eventDialogue = new EventDialogue(eventText);
        Debug.Log(fileName.Split('\\', '/','.')[^2]);
        events.Add(fileName.Split('\\','/','.')[^2], eventDialogue);
    }

    //遍历文件夹
    public void GetFiles()
    {
        string path = Application.streamingAssetsPath;
        DirectoryInfo folder = new DirectoryInfo(path);
        foreach (FileInfo file in folder.GetFiles(@"*.txt",SearchOption.AllDirectories))
        {
            Debug.Log(file.FullName);
            ReadEventFile(file.FullName);
        }
    }

    public void SetDialogueText(string content)
    {
        string[] slices = content.Split('：',':');

        if (slices.Length == 1)
        {
            if (slices[0][0] == '#')
            {
                SetEventWithDisplay(slices[0][1..^1]);
            }
            else
            {
                dialoguePage.character.text = currentCharacter;
                dialoguePage.content.text = slices[0];
            }

        }
        else if(slices.Length==2)
        {
            currentCharacter = slices[0];
            dialoguePage.character.text = slices[0];
            dialoguePage.content.text = slices[1];
        }
    }

    public void SetEventWithoutDisplay(string eventName)
    {
        //foreach(var item in events)
        //{
        //    Debug.Log(item.Key+" "+ item.Key.Length.ToString());
        //}
        //Debug.Log(eventName+" "+eventName.Length.ToString());
        //currentEvent = events[eventName];
        //currentIndex = 0;
        //if (dialoguePage == null)
        //{
        //    dialoguePage = Instantiate(dialoguePagePrefab, FindObjectOfType<Canvas>().transform);
        //}
        //SetDialogueText(currentEvent.content[currentIndex]);
    }

    public void SetEventWithDisplay(string eventName)
    {
        currentEvent = events[eventName];
        currentIndex = 0;
        if (dialoguePage == null)
        {
            dialoguePage = Instantiate(dialoguePagePrefab, FindObjectOfType<Canvas>().transform);
        }
        SetDialogueText(currentEvent.content[currentIndex]);
    }
}

public class EventDialogue
{
    public List<string> content=new List<string>();
    public EventDialogue(string content)
    {
        string[] contents = content.Split('\n');
        for (int i = 0; i < contents.Length; i++)
        {

            this.content.Add(contents[i]);
        }
    }
}

public class EventDialogueElement
{
    public string character;
    public string content;
}
