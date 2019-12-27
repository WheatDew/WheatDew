using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMindProperty : MonoBehaviour
{
    public Dictionary<string, float> Mind=new Dictionary<string, float>();
    public HashSet<string> ReceivedWords = new HashSet<string>();
}
