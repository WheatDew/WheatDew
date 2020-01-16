using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayButtonHideClick : MonoBehaviour
{
    public Transform target;
    private Button btn;
    private bool clickFlag=true;

    private void Start()
    {
        btn = GetComponent<Button>();
        target.transform.localScale = Vector3.zero;

        btn.onClick.AddListener(delegate { target.localScale = Vector3.one; clickFlag = false; });
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)&&clickFlag)
            target.localScale = Vector3.zero;
        clickFlag = true;
    }
}
