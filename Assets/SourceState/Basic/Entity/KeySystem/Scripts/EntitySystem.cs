using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySystem : MonoBehaviour
{

    #region �����б�

    public static Dictionary<string,EntityComponent> entities = new Dictionary<string,EntityComponent>();

    #endregion



    #region �ⲿ���ú���

    //����ID
    public static void DistributeKey(EntityComponent component)
    {
        string tempKey = null;
        while (true)
        {
            tempKey = Random.Range(int.MinValue, int.MaxValue).ToString();
            if (entities.ContainsKey(component.key))
            {
                continue;
            }
            component.key = tempKey;
            entities.Add(component.key, component);
            return;
        }  
    }

    #endregion
}
