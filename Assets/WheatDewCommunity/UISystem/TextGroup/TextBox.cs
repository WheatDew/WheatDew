using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

//显示对话文字的UI
public class TextBox : MonoBehaviour
{

    public Text textBox;
    public DisplayDialogueSystem displayDialogueSystem = World.Active.GetExistingSystem<DisplayDialogueSystem>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayDialogueJob();
    }

    private void DisplayDialogueJob()
    {
        textBox.text = displayDialogueSystem.GetDialogueForUI(1, 1);
    }
}
