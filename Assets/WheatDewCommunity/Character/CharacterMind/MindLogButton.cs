using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class MindLogButton : MonoBehaviour
{
    CharacterMindSystem characterMindSystem;
    public UnityEngine.UI.Text displayText;

    void Start()
    {
        characterMindSystem = World.Active.GetExistingSystem<CharacterMindSystem>();

        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate
        {
            displayText.text += characterMindSystem.GetMindLog();
        }); 
    }
}
