using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Cinemachine;

public class RDialogue : MonoBehaviour
{
    [SerializeField] private PDialogue dialoguePagePrefab;
    [HideInInspector] public PDialogue dialoguePage;

    private Dictionary<string,EventDialogue> events=new Dictionary<string, EventDialogue>();
    private Dictionary<string,Texture2D> pictures=new Dictionary<string, Texture2D>();

    public EventDialogue currentEvent;
    public string currentCharacter="";
    public int currentIndex;

    public CinemachineFreeLook cinemachineFreeLook;

    private void Start()
    {
        cinemachineFreeLook.enabled = false;
        ReadPicture();
        GetFiles();
        SCommand.Declare(@"设置 事件 为 \S+?", SetCurrentEvent);
        SCommand.Declare(@"显示 下一句", SetNextContent);
        SCommand.Execute("设置 事件 为 测试1");

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
        if (currentIndex < currentEvent.content.Count)
        {
            cinemachineFreeLook.enabled = false;
            SetDialogueText(currentEvent.content[currentIndex]);
        }
        else
        {
            cinemachineFreeLook.enabled = true;
            Destroy(dialoguePage.gameObject);
        }

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
        string path = Application.streamingAssetsPath+"/Events";
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
            Debug.Log("执行字符串" + slices[0]);
            if (slices[0][0] == '#')
            {
                string[] command = slices[0].Split('#');
                //Debug.LogFormat("{0} {1}", command[1], command[1].Length);
                switch (command[1].Trim('\0'))
                {
                    case "跳转":
                        SetEventWithDisplay(command[2]);
                        break;
                    case "立绘震动":
                        SetShake(dialoguePage.picture.transform);
                        currentIndex++;
                        SetDialogueText(currentEvent.content[currentIndex]);
                        break;
                    case "选项":
                        SetSelection(command);
                        break;
                }

            }
            else
            {
                if (!pictures.ContainsKey(currentCharacter))
                {
                    dialoguePage.picture.color = new Color(1, 1, 1, 0);
                }
                dialoguePage.character.text = currentCharacter;
                dialoguePage.content.text = slices[0];
            }

        }
        else if(slices.Length==2)
        {
            currentCharacter = slices[0];
            if (pictures.ContainsKey(currentCharacter))
            {
                dialoguePage.picture.color = new Color(1, 1, 1, 1);
                dialoguePage.picture.sprite = Sprite.Create(pictures[currentCharacter], new Rect(0, 0, pictures[currentCharacter].width, pictures[currentCharacter].height), Vector2.zero);
            }
            else
                dialoguePage.picture.color = new Color(1, 1, 1, 0);

            dialoguePage.character.text = slices[0];
            dialoguePage.content.text = slices[1];
        }
    }


    public void SetEventWithDisplay(string eventName)
    {
        if (events.ContainsKey(eventName))
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

    public void ReadPicture()
    {
        string path = Application.streamingAssetsPath+"/Pictures";
        DirectoryInfo folder = new DirectoryInfo(path);
        foreach (FileInfo file in folder.GetFiles(@"*.png", SearchOption.AllDirectories))
        {
            pictures.Add(file.FullName.Split('\\', '/', '.')[^2], GetTexture2D(file.FullName));
            Debug.Log(file.FullName.Split('\\', '/', '.')[^2]);
        }
    }

    public Texture2D GetTexture2D(string imgPath)
    {

        FileStream files = new FileStream(imgPath, FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();

        Texture2D tex = new(100, 100);
        tex.LoadImage(imgByte);
        tex.Apply();
        return tex;
    }

    //震动
    public async void SetShake(Transform target)
    {
        RectTransform rtransform = target.GetComponent<RectTransform>();
        for(int i = 0; i < 20; i++)
        {
            rtransform.localPosition -= Vector3.right*5;
            await new WaitForFixedUpdate();
            rtransform.localPosition += Vector3.right * 5;
            await new WaitForFixedUpdate();
        }
    }

    public void SetSelection(string[] command)
    {
        dialoguePage.SetSelection(command);
        dialoguePage.enableClick = false;
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
