using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCharacter : MonoBehaviour
{
    private static DCharacter _s;
    public static DCharacter s { get { return _s; } }

    private void Awake()
    {
        if (_s == null) _s = this;
    }

    public Dictionary<string, CCharacter> character_dict = new Dictionary<string, CCharacter>();
    public Dictionary<string , CCharacter> player_dict = new Dictionary<string , CCharacter>();
    public Dictionary<string, CCharacter> ai_dict = new Dictionary<string, CCharacter>();
}
