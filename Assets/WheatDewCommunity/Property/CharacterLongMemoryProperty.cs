using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLongMemoryProperty : MonoBehaviour
{
    public HashSet<string> LongMemory = new HashSet<string>();
}

public struct LongMemory_word
{
    public string word;
}

public struct LongMemory_SendSentence
{
    public string sentence;
}
