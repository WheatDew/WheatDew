using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VocabularyCard : MonoBehaviour
{
    public string word;
    public CreateSendWordCommandButton createSendWordCommandButton;

    private void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(delegate
        {
            createSendWordCommandButton.words.Add(word);
        });
    }
}
