using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

public class CreateSendWordCommandButton : MonoBehaviour
{
    public SendWordCommand sendWordCommandPrefab;
    public int originCharacter;
    public int targetCharacter;
    public Transform buttonVector;
    public string b_card="";
    public string g_card="";
    public string y_card="";
    public HashSet<VocabularyCard> blueCards = new HashSet<VocabularyCard>();
    public HashSet<VocabularyCard> greenCards = new HashSet<VocabularyCard>();
    public HashSet<VocabularyCard> yellowCards = new HashSet<VocabularyCard>();

    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(CreateSendWordCommand);
    }

    private void CreateSendWordCommand()
    {
        for(int i = 0; i < buttonVector.childCount; i++)
        {
            buttonVector.GetChild(i).GetComponent<VocabularyCard>().ResetColor();
        }

        if (originCharacter < 0 || targetCharacter < 0)
        {
            Debug.Log("创建SendWordCommand失败,指定对象为空");
            return;
        }

        SendWordCommand swc = Instantiate(sendWordCommandPrefab);
        swc.gameObject.AddComponent<GameObjectEntity>();
        swc.origin = originCharacter;
        swc.target = targetCharacter;
        HashSet<string> wordset = new HashSet<string>();
        if (b_card != "" & g_card != "")
        {
            wordset.Add(b_card);
            wordset.Add(g_card);
        }
        if(y_card!="")
        wordset.Add(y_card);

        swc.context = new HashSet<string>(wordset);

        Debug.Log("创建SendWordComman成功,内容为" + string.Format("{0},{1},{2}",b_card,g_card,y_card));
        b_card = "";
        g_card = "";
        y_card = "";
    }

}
