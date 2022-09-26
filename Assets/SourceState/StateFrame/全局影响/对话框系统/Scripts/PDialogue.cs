using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class PDialogue : MonoBehaviour,IPointerClickHandler
{
    public TMP_Text content,character;
    public Image picture;
    public PSelection selectionPrefab;
    public Transform selectionParent;

    public bool enableClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(enableClick)
            SCommand.Execute("显示 下一句");
    }

    public void SetSelection(string[] command)
    {
        for(int i = 3; i < command.Length; i += 2)
        {
            PSelection selection = Instantiate(selectionPrefab, selectionParent);
            selection.page = this;
            selection.content.text = command[i-1];
            selection.SetClick(string.Format("设置 事件 为 {0}", command[i]));
        }
    }

    public void ClearSelection()
    {
        for (int i = 0; i < selectionParent.childCount; i++)
        {
            Destroy(selectionParent.GetChild(i).gameObject);
        }
    }
}
