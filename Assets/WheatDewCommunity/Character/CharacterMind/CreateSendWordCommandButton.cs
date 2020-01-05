using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CreateSendWordCommandButton : MonoBehaviour
{
    public HashSet<string> words=new HashSet<string>();
    public SendWordCommand sendWordCommandPrefab;
    public int originCharacter;
    public int targetCharacter;

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CreateSendWordCommand);
    }

    private void CreateSendWordCommand()
    {
        if (originCharacter < 0 || targetCharacter < 0)
        {
            Debug.Log("创建SendWordCommand失败,指定对象为空");
            return;
        }

        SendWordCommand swc = Instantiate(sendWordCommandPrefab);
        swc.gameObject.AddComponent<GameObjectEntity>();
        swc.origin = originCharacter;
        swc.target = targetCharacter;
        swc.context = new HashSet<string>(words);
        string s = "";
        foreach( var item in words) { s += item+" "; };
        Debug.Log("创建SendWordComman成功,内容为" + s);
        words.Clear();
    }
}
