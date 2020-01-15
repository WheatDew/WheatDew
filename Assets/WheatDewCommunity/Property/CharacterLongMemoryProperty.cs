using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLongMemoryProperty : MonoBehaviour
{
    Dictionary<string, LongMemory_word> LongMemory = new Dictionary<string, LongMemory_word>();
}

public struct LongMemory_word
{
    public string word;
}

public struct LongMemory_SendSentence
{
    public string sentence;
}
