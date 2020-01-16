using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayButton : MonoBehaviour
{
    private Button btn;
    public Transform target;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(delegate { target.localScale = Vector3.one; });
    }
}
