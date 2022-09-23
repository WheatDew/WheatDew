using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RDialogue : MonoBehaviour
{
    [SerializeField] private PDialogue dialoguePagePrefab;
    [HideInInspector] public PDialogue dialoguePage;

    private Dictionary<string,EventDialogue> events=new Dictionary<string, EventDialogue>();

    public EventDialogue currentEvent;
    public int currentIndex;

    private void Start()
    {
        EventDialogue eventDialogue = new EventDialogue("�Ի�����1\n�Ի�����2\n�Ի�����3\n");
        events.Add("����", eventDialogue);
        SCommand.Declare(@"���� �¼� Ϊ \S+?", SetCurrentEvent);
        SCommand.Declare(@"��ʾ ��һ��", SetNextContent);
        SCommand.Execute("���� �¼� Ϊ ����");
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
            dialoguePage.content.text = currentEvent.content[currentIndex];
        else
            Destroy(dialoguePage.gameObject);
    }

    public void SetCurrentEvent(string value, CommandData commandData)
    {
        Debug.Log(value);
        string[] values=value.Split(' ');
        currentEvent = events[values[3]];
        if (dialoguePage == null)
        {
            dialoguePage = Instantiate(dialoguePagePrefab, FindObjectOfType<Canvas>().transform);
        }
        dialoguePage.content.text = currentEvent.content[currentIndex];
        Debug.Log(currentEvent.content[currentIndex]);
    }

    public void CloseDialogue(string value,CommandData commandData)
    {
        Destroy(dialoguePage.gameObject);
    }
}

public class EventDialogue
{
    public List<string> content=new List<string>();
    public EventDialogue(string content)
    {
        string[] contents = content.Split('\n');
        for(int i = 0; i < contents.Length; i++)
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
