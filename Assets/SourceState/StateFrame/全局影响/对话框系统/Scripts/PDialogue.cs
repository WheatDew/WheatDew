using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class PDialogue : MonoBehaviour,IPointerClickHandler
{
    public TMP_Text content,character;

    public void OnPointerClick(PointerEventData eventData)
    {
        SCommand.Execute("отй╬ обр╩╬Д");
    }
}
