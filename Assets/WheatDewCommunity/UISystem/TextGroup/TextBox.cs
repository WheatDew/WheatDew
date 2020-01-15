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

        displayDialogueSystem.GetDialogueForUI(1, 2,ref content, ref life);

        if (life > 0)
        {
            textBox.text = content;
        }
        else
        {
            textBox.text = "";
        }

        life -= p_Timer.currentDeltaTime;
    }

}
