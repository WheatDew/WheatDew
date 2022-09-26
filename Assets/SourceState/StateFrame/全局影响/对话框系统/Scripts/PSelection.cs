using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PSelection : MonoBehaviour
{
    [HideInInspector] public PDialogue page; 
    public TMP_Text content;


    public void SetClick(string command)
    {
        GetComponent<Button>().onClick.AddListener(delegate
        {
            SCommand.Execute(command);
            page.enableClick = true;
            page.ClearSelection();
        });
    }
}
