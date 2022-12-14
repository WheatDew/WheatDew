using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroup : MonoBehaviour
{
    public Dictionary<string, CCharacter> characterList = new Dictionary<string, CCharacter>();
    public List<StrollPoint> strollPoints=new List<StrollPoint>();

    public void BuildingTask()
    {
        
    }
}
