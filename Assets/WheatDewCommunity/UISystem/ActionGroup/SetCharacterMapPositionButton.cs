using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class SetCharacterMapPositionButton : MonoBehaviour
{
    private Button btn;
    private CharacterMapPositionSystem s_characterMapPosition;
    public BackgroundImageController backgroundImageController;
    public string mapName;

    private void Start()
    {
        btn = GetComponent<Button>();
        s_characterMapPosition = World.Active.GetExistingSystem<CharacterMapPositionSystem>();
        btn.onClick.AddListener(delegate { if(s_characterMapPosition!=null)s_characterMapPosition.SetCharacterMapPosition(mapName);
            backgroundImageController.UpdateBackground(mapName);
        });  
    }
}
