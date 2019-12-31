using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayHideButton : MonoBehaviour
{
    private Button btn;
    public Transform target;

    // Start is called before the first frame update
    void Start()
    {
        btn = GetComponent<Button>();
        Hide();
    }

    public void Display()
    {
        target.localScale = Vector3.one;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Hide);
    }

    public void Hide()
    {
        target.localScale = Vector3.zero;
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(Display);
    }
}
