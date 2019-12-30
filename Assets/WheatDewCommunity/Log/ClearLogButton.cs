using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearLogButton : MonoBehaviour
{

    public UnityEngine.UI.Text targetText;
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate
        {
            targetText.text = "";
        });
    }

}
