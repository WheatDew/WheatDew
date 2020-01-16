using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignScene : MonoBehaviour
{
    public string sceneName;
    public BackgroundImageController bgiController;
    private UnityEngine.UI.Button btn;

    private void Start()
    {
        btn = GetComponent<UnityEngine.UI.Button>();
        btn.onClick.AddListener(delegate { bgiController.UpdateBackground(sceneName); }); 
    }
}
