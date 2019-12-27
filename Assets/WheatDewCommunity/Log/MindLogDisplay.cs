using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

public class MindLogDisplay : MonoBehaviour
{
    MindLogSystem mindLogSystem;
    public Text mindLogDisplay;
    void Start()
    {
        mindLogSystem = World.Active.GetExistingSystem<MindLogSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        mindLogDisplay.text = mindLogSystem.mindLog;
    }
}
