using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using Cinemachine;
using UnityEngine.Events;

public class RDialogue : MonoBehaviour
{
    [SerializeField] private PDialogue dialoguePagePrefab;
    [HideInInspector] public PDialogue dialoguePage;

    private Dictionary<string,EventDialogue> events=new Dictionary<string, EventDialogue>();
    private Dictionary<string,Texture2D> pictures=new Dictionary<string, Texture2D>();
    private Dictionary<string, Texture2D> icons = new Dictionary<string, Texture2D>();
    private Dictionary<string, Texture2D> backgrounds = new Dictionary<string, Texture2D>();
    public Dictionary<string, string> convert = new Dictionary<string, string>();

    public EventDialogue currentEvent;
    public string currentCharacter="";
    public int currentIndex;

    public CinemachineFreeLook cinemachineFreeLook;

    private void Start()
    {
        if(cinemachineFreeLook != null)
            cinemachineFreeLook.enabled = false;
        //读取图片
        ReadPicture();
        //读取事件文件
        ReadEventFiles();
        //读取转换字符串
        GetConvertString();
        //读取图标文件
        ReadIcons();
        //读取场景
        ReadBackgrounds();
        SCommand.Declare(@"设置 事件 为 \S+?", SetCurrentEvent);
        SCommand.Declare(@"显示 下一句", SetNextContent);
        SCommand.Execute("设置 事件 为 test3");

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
            if(cinemachineFreeLook!=null)
                cinemachineFreeLook.enabled = false;
            SetDialogueText(currentEvent.content[currentIndex]);
        }
        else
        {
            if (cinemachineFreeLook != null)
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
        //Debug.Log(fileName.Split('\\', '/','.')[^2]);
        events.Add(fileName.Split('\\','/','.')[^2], eventDialogue);
    }

    //读取转换字符串
    public void ReadCovertStringFile(string fileName)
    {
        string convert_string = File.ReadAllText(fileName);
        string[] convert_strings = convert_string.Split('\n');
        for(int i=0;i< convert_strings.Length; i++)
        {
            
            string[] convert_strings_slices = convert_strings[i].Split(',', '，');
            convert.Add(convert_strings_slices[0], convert_strings_slices[1]);
            //Debug.LogFormat("加载转换字符串：{0}to{1}", convert_strings_slices[0], convert_strings_slices[1]);
        }
    }

    //遍历文件夹
    public void ReadEventFiles()
    {
        string path = Application.streamingAssetsPath+"/Events";
        DirectoryInfo folder = new DirectoryInfo(path);
        foreach (FileInfo file in folder.GetFiles(@"*.txt",SearchOption.AllDirectories))
        {
            //Debug.LogFormat("读取故事文件路径：{0}",file.FullName);
            ReadEventFile(file.FullName);
        }
    }

    public void GetConvertString()
    {
        string path = Application.streamingAssetsPath + "/Convert";
        DirectoryInfo folder = new DirectoryInfo(path);
        foreach (FileInfo file in folder.GetFiles(@"*.txt", SearchOption.AllDirectories))
        {
            //Debug.LogFormat("读取转换字符串文件路径：{0}",file.FullName);
            ReadCovertStringFile(file.FullName);
        }
    }

    public void SetDialogueText(string content)
    {
        string[] slices = content.Split('：',':');

        UnityAction next = delegate
        {
            currentIndex++;
            SetDialogueText(currentEvent.content[currentIndex]);
        };

        if (slices.Length == 1)
        {
            Debug.Log("执行字符串命令:" + slices[0]);
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
                        next();
                        break;
                    case "选项":
                        SetSelection(command);
                        break;
                    case "显示立绘":
                        if (command.Length == 4)
                            DisplayPicture(command[2], "0", "0");
                        else if (command.Length == 5)
                        {
                            DisplayPicture(command[2], command[3], "0");
                        }
                        else if (command.Length == 6)
                            DisplayPicture(command[2], command[3], command[4]);
                        next();
                        break;
                    case "设置位置":
                        if (command.Length == 4)
                            SetPicturePosition(new Vector3(float.Parse(command[2]), 0, 0));
                        else if(command.Length == 5)
                            SetPicturePosition(new Vector3(float.Parse(command[2]), float.Parse(command[3]), 0));
                        break;
                    case "移动立绘":
                        if (command.Length == 4)
                            MovePicture(command[2], "0", "0.5");
                        else if (command.Length == 5)
                            MovePicture(command[2], command[3], "0.5");
                        else if (command.Length == 6)
                            MovePicture(command[2], command[3], command[4]);
                        next();
                        break;
                    case "隐藏立绘":
                        HiddenPicture();
                        next();
                        break;
                    case "隐藏图标":
                        HiddenIcon();
                        next();
                        break;
                    case "警觉":
                        DisplayIcon("Perceive", "flicker");
                        if (command.Length > 3 && command[2]=="连续")
                            next();
                        break;
                    case "显示场景":
                        DisplayBackground(command[2]);
                        next();
                        break;
                    case "隐藏场景":
                        HiddenBackground();
                        next();
                        break;

                }

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
            //Debug.Log(file.FullName.Split('\\', '/', '.')[^2]);
        }
    }

    public void ReadIcons()
    {
        string path = Application.streamingAssetsPath + "/Icons";
        DirectoryInfo folder = new DirectoryInfo(path);
        foreach (FileInfo file in folder.GetFiles(@"*.png", SearchOption.AllDirectories))
        {
            icons.Add(file.FullName.Split('\\', '/', '.')[^2], GetTexture2D(file.FullName));
            //Debug.Log(file.FullName.Split('\\', '/', '.')[^2]);
        }
    }

    public void ReadBackgrounds()
    {
        string path = Application.streamingAssetsPath + "/Backgrounds";
        DirectoryInfo folder = new DirectoryInfo(path);
        foreach (FileInfo file in folder.GetFiles(@"*.png", SearchOption.AllDirectories))
        {
            backgrounds.Add(file.FullName.Split('\\', '/', '.')[^2], GetTexture2D(file.FullName));
            //Debug.Log(file.FullName.Split('\\', '/', '.')[^2]);
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

    //选项
    public void SetSelection(string[] command)
    {
        dialoguePage.SetSelection(command);
        dialoguePage.enableClick = false;
    }

    //显示立绘
    public void DisplayPicture(string character,string pos_x,string pos_y)
    {
        if (convert.ContainsKey(character) && pictures.ContainsKey(convert[character]))
        {
            dialoguePage.picture.color = new Color(1, 1, 1, 1);
            dialoguePage.picture.sprite = Sprite.Create(pictures[convert[character]], new Rect(0, 0, pictures[convert[character]].width, pictures[convert[character]].height), Vector2.zero);
            Debug.Log(pos_x);
            dialoguePage.picture.rectTransform.localPosition = new Vector3(float.Parse(pos_x),float.Parse(pos_y),0);
        }
    }

    //立绘移动(移动方向上的长度)
    public async void MovePicture(string x,string y,string t)
    {
        Debug.LogFormat("设置x轴位置:{0};设置值长度:{1}",x,x.Length);
        Vector3 direction = new Vector3(float.Parse(x), float.Parse(y));
        float totalTime =float.Parse(t);
        Vector3 targetPosition = dialoguePage.picture.rectTransform.localPosition+direction;
        Vector3 startPosition = dialoguePage.picture.rectTransform.localPosition;
        float currentTime=0;
        while (currentTime < totalTime)
        {
            dialoguePage.picture.rectTransform.localPosition = Vector3.Lerp(startPosition, targetPosition, currentTime / totalTime);
            currentTime+=Time.deltaTime;
            await new WaitForUpdate();
        }
    }

    //设置立绘位置
    public void SetPicturePosition(Vector3 pos)
    {
        dialoguePage.picture.rectTransform.localPosition = pos;
    }

    //隐藏立绘
    public void HiddenPicture()
    {
        dialoguePage.picture.color = new Color(1, 1, 1, 0);
    }

    //显示符号
    public async void DisplayIcon(string iconName,string aniName)
    {
        dialoguePage.icon.sprite = Sprite.Create(icons[iconName], new Rect(0, 0, icons[iconName].width, icons[iconName].height), Vector2.zero);

        if (aniName!=null)
        {
            dialoguePage.icon.color = new Color(1, 1, 1, 1);
        }
        if (aniName == "flicker")
        {
            dialoguePage.icon.color = new Color(1, 1, 1, 0);
            await new WaitForSeconds(0.05f);
            dialoguePage.icon.color = new Color(1, 1, 1, 1);
            await new WaitForSeconds(0.05f);
            dialoguePage.icon.color = new Color(1, 1, 1, 0);
            await new WaitForSeconds(0.05f);
            dialoguePage.icon.color = new Color(1, 1, 1, 1);
            await new WaitForSeconds(0.05f);
            dialoguePage.icon.color = new Color(1, 1, 1, 0);
            await new WaitForSeconds(0.05f);
            dialoguePage.icon.color = new Color(1, 1, 1, 1);
        }
    }

    public void HiddenIcon()
    {
        dialoguePage.icon.color=new Color(1, 1, 1, 0);
    }

    public void DisplayBackground(string backgroundName)
    {
        dialoguePage.scene.sprite = Sprite.Create(backgrounds[backgroundName], new Rect(0, 0, backgrounds[backgroundName].width, backgrounds[backgroundName].height), Vector2.zero);
        dialoguePage.scene.gameObject.SetActive(true);
    }

    public void HiddenBackground()
    {
        dialoguePage.scene.gameObject.SetActive(false);
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
