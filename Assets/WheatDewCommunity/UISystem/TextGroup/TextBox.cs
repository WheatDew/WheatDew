using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

//显示对话文字的UI
public class TextBox : MonoBehaviour
{
    public GameObject textGroup;
    public Text textBox;
    public DisplayDialogueSystem displayDialogueSystem;
    private float life=0;
    private string content="";
    private TimerProperty p_Timer;

    // Start is called before the first frame update
    void Start()
    {
        displayDialogueSystem = World.Active.GetExistingSystem<DisplayDialogueSystem>();
        p_Timer = GetComponent<TimerProperty>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayDialogueJob();
    }

    private void DisplayDialogueJob()
    {
        string t_Content;
        float t_Life;
        displayDialogueSystem.GetDialogueForUI(1, 2, out t_Content, out t_Life);

        if(t_Life!=-1f)
        {
            content = t_Content;
            life = t_Life;
            Debug.Log(string.Format("set content={0},life={1}", content, life));
        }

        if (life > 0)
        {
            textBox.text = content;
        }
        else
        {
            textBox.text = "(空)";
        }

        life -= p_Timer.currentDeltaTime;
    }

}
