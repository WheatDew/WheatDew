using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SCharacter
{
    private static Dictionary<string, CCharacter> characters = new Dictionary<string, CCharacter>();

    public static void AddCharacter(string key,CCharacter character)
    {
        characters.Add(key, character);
    }

    public static void AddCharacter(CCharacter character)
    {
        characters.Add(character.transform.GetInstanceID().ToString(), character);
    }
}
