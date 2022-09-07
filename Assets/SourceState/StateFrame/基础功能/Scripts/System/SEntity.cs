using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SEntity
{
    private static Dictionary<string,CEntity> components = new Dictionary<string,CEntity>();
    private static Dictionary<string, HashSet<string>> componentTags = new Dictionary<string, HashSet<string>>();

    public static void Add(string key,CEntity component)
    {
        components.Add(key,component);
        for(int i = 0; i < component.tags.Count; i++)
        {
            if (!componentTags.ContainsKey(component.tags[i]))
            {
                HashSet<string> temp = new HashSet<string>();
                temp.Add(key);
                componentTags.Add(component.tags[i],temp);
            }
            else
            {
                componentTags[component.tags[i]].Add(key);
            }
        }
    }
    public static HashSet<string> GetKeyByTag(string tag)
    {
        return componentTags[tag];
    }
}
